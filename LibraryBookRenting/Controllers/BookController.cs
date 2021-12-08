using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using LibraryBookRenting.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Controllers
{
    [ApiController]
    [Authorize]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet(ApiRoutes.UserRoutes.GetAll)]
        public IActionResult Get()
        {
            return Ok(_bookService.GetAll());
        }

        [HttpPut(ApiRoutes.UserRoutes.GetAll)]
        public IActionResult Update(Guid bookId, [FromBody] CreateUserRequest request)
        {
            IActionResult actionResult;
            ErrorModel errors = new ErrorModel();

            _bookService.UpdateBook(bookId, request, ref errors);

            if (errors.IsEmpty)
            {
                return Ok(bookId);
            }
            else
            {
                return BadRequest(errors);
            }
        }
    }
}
