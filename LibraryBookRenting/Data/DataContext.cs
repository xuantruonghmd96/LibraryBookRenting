using LibraryBookRenting.Domain;
using LibraryBookRenting.Domain.Infastructure;
using LibraryBookRenting.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryBookRenting.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book> Books { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var entitiesAssembly = typeof(IEntity).Assembly;
            builder.RegisterEntityTypeConfiguration(entitiesAssembly);
            builder.AddRestrictDeleteBehaviorConvention();
        }
    }
}
