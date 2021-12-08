using LibraryBookRenting.Domain.Infastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public virtual int CreditCount { get; set; }
    }
}
