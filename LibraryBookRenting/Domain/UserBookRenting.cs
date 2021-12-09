using LibraryBookRenting.Domain.Infastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Domain
{
    public class UserBookRenting : BaseEnity<Guid>
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Guid BookId { get; set; }
        public virtual Book Book { get; set; }

        public int Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }
    }

    public class UserBookRentingConfiguration : IEntityTypeConfiguration<UserBookRenting>
    {
        public void Configure(EntityTypeBuilder<UserBookRenting> builder)
        {
            builder
                .HasOne<ApplicationUser>(s => s.User)
                .WithMany(g => g.UserBookRentings)
                .HasForeignKey(s => s.UserId);

            builder
                .HasOne<Book>(s => s.Book)
                .WithMany(g => g.UserBookRentings)
                .HasForeignKey(s => s.BookId);
        }
    }
}
