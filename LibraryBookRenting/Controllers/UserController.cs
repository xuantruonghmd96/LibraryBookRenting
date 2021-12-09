using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
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
        public IActionResult RentBooks([FromBody] RentBooksRequest request)
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
                var bookRented = _userService.RentBooks(request, ref errors);
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
    }
}
