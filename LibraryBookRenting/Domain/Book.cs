using LibraryBookRenting.Domain.Infastructure;
using LibraryBookRenting.Installers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBookRenting.Domain
{
    public class Book : BaseEnity<Guid>
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<UserBookRenting> UserBookRentings { get; set; }
    }

    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(x => x.Quantity)
                .HasDefaultValue(MyAppSettings.BussinessConfiguration.BookQuantityInStock);
        }
    }
}
