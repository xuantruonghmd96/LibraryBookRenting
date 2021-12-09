using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Contracts.Requests
{
    public class RentBook
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }
    }

    public class RentBooksRequest
    {
        public List<RentBook> Books { get; set; }

        public RentBooksRequest()
        {
            Books = new List<RentBook>();
        }
    }
}
