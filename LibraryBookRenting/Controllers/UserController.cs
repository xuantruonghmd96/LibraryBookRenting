using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Services.Interfaces;
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

        [HttpPost(ApiRoutes.UserRoutes.GetAll)]
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
    }
}
