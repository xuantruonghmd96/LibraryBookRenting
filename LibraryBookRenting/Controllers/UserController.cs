using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using LibraryBookRenting.Extensions;
using LibraryBookRenting.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Signup
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.UserRoutes.Create)]
        public async Task<IActionResult> Signup([FromBody] CreateUserRequest request)
        {
            var authResponse = await _userService.SignupAsync(request.UserName, request.Password);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse);
            }
            else
            {
                return Ok(authResponse);
            }
        }

        /// <summary>
        /// Signin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.UserRoutes.Signin)]
        public async Task<IActionResult> Signin([FromBody] CreateUserRequest request)
        {
            var authResponse = await _userService.SigninAsync(request.UserName, request.Password);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse);
            }
            else
            {
                return Ok(authResponse);
            }
        }

        /// <summary>
        /// Rent Books
        /// </summary>
        /// <param name="request">List BookId</param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.UserRoutes.RentBook)]
        [Authorize]
        public async Task<IActionResult> RentBooks([FromBody] RentBooksRequest request)
        {
            IActionResult actionResult;
            ErrorModel errors = new ErrorModel();

            if (!ModelState.IsValid)
            {
                AddErrorsFromModelState(ref errors);
                actionResult = BadRequest(errors.ErrorMessages);
            }
            else
            {
                var userId = HttpContext.GetUserId();
                var bookRented = await _userService.RentBooks(userId, request, errors);
                if (errors.IsEmpty)
                {
                    actionResult = Ok(bookRented);
                }
                else
                {
                    actionResult = BadRequest(errors);
                }
            }

            return actionResult;
        }

        /// <summary>
        /// Get List Book of User, 
        /// UserId will taked in JWT Token, so the user can only see their books
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.UserRoutes.GetBook)]
        [Authorize]
        public async Task<IEnumerable<BookResponse>> GetBookRented()
        {
            var userId = HttpContext.GetUserId();
            return _userService.GetBookRented(userId);
        }

        /// <summary>
        /// Get List Book of User will expire in a week
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.UserRoutes.GetBookExpireInWeek)]
        [Authorize]
        public async Task<IEnumerable<BookResponse>> GetBookExpireInWeek()
        {
            var userId = HttpContext.GetUserId();
            return _userService.GetBookRented(userId, DateTime.Now.Date, DateTime.Now.Date.AddDays(7));
        }

        /// <summary>
        /// Get List Book of User that has expired
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.UserRoutes.GetBookExpired)]
        [Authorize]
        public async Task<IEnumerable<BookResponse>> GetBookExpired()
        {
            var userId = HttpContext.GetUserId();
            return _userService.GetBookRented(userId, null, DateTime.Now.Date);
        }

        /// <summary>
        /// Get List Book of User that is not expired
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.UserRoutes.GetBookNotExpired)]
        [Authorize]
        public async Task<IEnumerable<BookResponse>> GetBookNotExpired()
        {
            var userId = HttpContext.GetUserId();
            return _userService.GetBookRented(userId, DateTime.Now.Date, null);
        }
    }
}
