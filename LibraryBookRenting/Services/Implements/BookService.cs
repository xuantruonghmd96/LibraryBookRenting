using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using LibraryBookRenting.Domain;
using LibraryBookRenting.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Services.Implements
{
    public class BookService : IBookService
    {
        private readonly List<Book> _listBook;

        public BookService()
        {
            _listBook = new List<Book>();
            for (int i = 1; i <= 4; i++)
            {
                _listBook.Add(new Book { Id = new Guid(), Name = string.Concat("Book ", i)});
            }
        }

        public IEnumerable<BookResponse> GetAll()
        {
            return _listBook.Select(x => new BookResponse(x));
        }

        public void UpdateBook(Guid bookId, CreateUserRequest request, ref ErrorModel errors)
        {
            throw new NotImplementedException();
        }
    }
}
