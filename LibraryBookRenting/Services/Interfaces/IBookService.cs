using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Services.Interfaces
{
    public interface IBookService
    {
        public IEnumerable<BookResponse> GetAll();
        void UpdateBook(Guid bookId, CreateUserRequest request, ref ErrorModel errors);
    }
}
