using LibraryBookRenting.Domain.Infastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        public virtual ICollection<UserBookRenting> UserBookRentings { get; set; }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.CreditCount)
                .HasDefaultValue(100);
        }
    }
}
