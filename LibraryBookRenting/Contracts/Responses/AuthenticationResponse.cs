using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Contracts.Responses
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> ErrorMessage { get; set; }
    }
}
