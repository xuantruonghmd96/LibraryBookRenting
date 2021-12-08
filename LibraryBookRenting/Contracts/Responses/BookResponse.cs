using LibraryBookRenting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Contracts.Responses
{
    public class BookResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public BookResponse(Book book)
        {
            this.Id = book.Id;
            this.Name = book.Name;
        }
    }
}
