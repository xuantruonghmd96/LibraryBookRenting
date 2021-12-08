using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Contracts
{
    public class ErrorModel
    {
        public List<string> ErrorMessages { get; set; }
        public bool IsEmpty { get => ErrorMessages.Count == 0; }
    }
}
