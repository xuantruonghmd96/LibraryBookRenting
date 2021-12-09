using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationResponse> SignupAsync(string userName, string password);
        Task<AuthenticationResponse> SigninAsync(string userName, string password);
        Task<int> RentBooks(string userId, RentBooksRequest request, ErrorModel errors);
        /// <summary>
        /// Get book of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="from">renting will greater or equal from</param>
        /// <param name="to">renting will less than to</param>
        /// <returns></returns>
        IEnumerable<BookResponse> GetBookRented(string userId, DateTime? from = null, DateTime? to = null);
    }
}
