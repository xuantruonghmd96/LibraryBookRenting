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
        public int Price { get; set; }
        public int Quantity { get; set; }

        public BookResponse()
        {
        }

        public BookResponse(Book book)
        {
            this.Id = book.Id;
            this.Name = book.Name;
            this.Price = book.Price;
            this.Quantity = book.Quantity;
        }
    }
}
