using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using LibraryBookRenting.Data;
using LibraryBookRenting.Domain;
using LibraryBookRenting.Extensions;
using LibraryBookRenting.Installers;
using LibraryBookRenting.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LibraryBookRenting.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<ApplicationUser> userManager, DataContext dbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponse> SignupAsync(string userName, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);

            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = new[] { "User name is exists" }
                };
            }

            var newUser = new ApplicationUser
            {
                UserName = userName,
                //CreditCount = 100,
            };
            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description)
                };
            }

            return GenerateAuthenticationResponse(newUser);
        }

        public async Task<AuthenticationResponse> SigninAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = new[] { "User does not exists" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResponse
                {
                    ErrorMessages = new[] { "Password does not match" }
                };
            }
            else
            {
                return GenerateAuthenticationResponse(user);
            }
        }

        private AuthenticationResponse GenerateAuthenticationResponse(ApplicationUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(MyAppSettings.JwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.UserName),
                    new Claim("id", newUser.Id),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResponse
            {
                IsSuccess = true,
                Token = tokenHandler.WriteToken(token),
            };
        }

        public async Task<int> RentBooks(string userId, RentBooksRequest request, ErrorModel errors)
        {
            request.Books = request.Books.Where(x => x.Quantity > 0).ToList();

            //Validate request infomation
            int quantityEveryBook = MyAppSettings.BussinessConfiguration.BookQuantityInStock;
            if (quantityEveryBook <= 0 || request.Books.Any(x => x.Quantity > quantityEveryBook))
            {
                errors.ErrorMessages.Add("Book quantity does not enough");
                return -1;
            }

            int amount = 0;
            Dictionary<Guid, int> dictBookPrice;
            var listRequestBookId = request.Books.Select(y => y.BookId).ToList();
            if (request.Books.Sum(x => x.Quantity) > 5)
            {
                errors.ErrorMessages.Add("User rent more than 5");
                return -1;
            }
            else
            {
                dictBookPrice = _dbContext.Books
                    .Where(x => listRequestBookId.Contains(x.Id))
                    .Select(x => new { x.Id, x.Price }).ToDictionary(x => x.Id, x => x.Price);

                if (request.Books.Any(x => !dictBookPrice.ContainsKey(x.BookId)))
                {
                    errors.ErrorMessages.Add("Book not found");
                    return -1;
                }
                else
                {
                    amount = request.Books.Sum(x => x.Quantity * dictBookPrice[x.BookId]);
                }
            }

            //Begin Transaction (Validate data and Submit renting)
            using (var scope = new TransactionScope(
               TransactionScopeOption.Required,
               new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                connection.Open();

                try
                {
                    var options = new DbContextOptionsBuilder<DataContext>()
                        .UseSqlServer(connection)
                        .Options;

                    using (var context = new DataContext(options))
                    {
                        var user = context.Users.Find(userId);
                        if (user == null)
                        {
                            errors.ErrorMessages.Add("User not found");
                        }
                        else if (user.CreditCount < amount)
                        {
                            errors.ErrorMessages.Add("User does not enough credits");
                        }
                        else
                        {
                            var userBookRentedQuantity = context.UserBookRentings.Where(x => x.UserId == userId).Sum(x => x.Quantity);
                            if (userBookRentedQuantity + request.Books.Sum(x => x.Quantity) > 5)
                            {
                                errors.ErrorMessages.Add("User rent more than 5");
                            }
                            else
                            {
                                Dictionary<Guid, int> dictRentingBook = context.UserBookRentings
                                    .Where(x => listRequestBookId.Contains(x.BookId))
                                    .GroupBy(x => x.BookId)
                                    .Select(x => new { Id = x.Key, Quantity = x.Sum(y => y.Quantity) })
                                    .ToDictionary(x => x.Id, x => x.Quantity);
                                foreach (var item in request.Books.GroupBy(x => x.BookId).Select(x => new RentBook { BookId = x.Key, Quantity = x.Sum(y => y.Quantity) }))
                                {
                                    int quantityRented = 0;
                                    if (dictRentingBook.ContainsKey(item.BookId))
                                    {
                                        quantityRented = dictRentingBook[item.BookId];
                                    }
                                    if (quantityRented + item.Quantity > quantityEveryBook)
                                    {
                                        errors.ErrorMessages.Add("Book quantity does not enough");
                                    }
                                }
                            }
                        }
                        if (errors.IsEmpty)
                        {
                            foreach (var item in request.Books)
                            {
                                context.UserBookRentings.Add(new UserBookRenting
                                {
                                    UserId = userId,
                                    BookId = item.BookId,
                                    Quantity = item.Quantity,
                                    ExpiredDate = item.ExpiredDate,
                                });
                            }
                            user.CreditCount -= amount;
                        }

                        context.SaveChanges();
                    }

                    // Commit transaction if all commands succeed, transaction will auto-rollback
                    // when disposed if either commands fails
                    scope.Complete();
                    return request.Books.Sum(x => x.Quantity);
                }
                catch (Exception e)
                {
                    // Transaction deadlock
                    errors.ErrorMessages.Add("Conflict! Please try again");
                    return -1;
                }
            }
        }

        public IEnumerable<BookResponse> GetBookRented(string userId, DateTime? from, DateTime? to)
        {
            var rentingOfUser = _dbContext.UserBookRentings
                .Where(x => x.UserId == userId
                    && (!from.HasValue || x.ExpiredDate >= from)
                    && (!to.HasValue || x.ExpiredDate < to)
                )
                .Select(x => new BookResponse
                {
                    Id = x.BookId,
                    Name = x.Book.Name,
                    Price = x.Book.Price,
                    Quantity = x.Quantity,
                }).ToList();

            return rentingOfUser.GroupBy(x => x.Id).Select(x => new BookResponse
            {
                Id = x.Key,
                Name = x.First().Name,
                Price = x.First().Price,
                Quantity = x.Sum(y => y.Quantity)
            });
        }

        public void SubstractCreditsEveryDay()
        {
            //TODO: write this raw SQL to StoredProcedure
            var dictUserCreditSubstract = _dbContext.Database.ExecuteSqlRaw(@"
                UPDATE AspNetUsers
                SET CreditCount = U.CreditCount - (USER_CREDIT.CreditCount / 5)
                FROM AspNetUsers AS U
                INNER JOIN
                (
	                SELECT UserId as UserId, SUM(AMOUNT) as CreditCount
	                FROM
	                (
		                SELECT R.UserId AS UserId, R.BookId AS BookId, SUM(R.Quantity * B.Price) AS AMOUNT
		                FROM UserBookRentings R
		                LEFT JOIN BOOKS B ON R.BookId = B.ID
		                WHERE R.ExpiredDate <= CONVERT(date, getdate())
		                GROUP BY R.USERID, R.BOOKID 
	                ) AS T
	                GROUP BY USERID
                ) AS USER_CREDIT ON USER_CREDIT.UserId = U.Id
            ");
        }
    }
}
