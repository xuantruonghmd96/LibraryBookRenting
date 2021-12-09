using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Domain
{
    [Keyless]
    public class UserCreditModel
    {
        public string UserId { get; set; }
        public int CreditCount { get; set; }
    }
}
