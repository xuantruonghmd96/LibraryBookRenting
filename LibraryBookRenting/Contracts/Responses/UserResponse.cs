using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Contracts.Responses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
