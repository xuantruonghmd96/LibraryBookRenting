using FluentAssertions;
using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using LibraryBookRenting.Data;
using LibraryBookRenting.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryBookRenting.IntegrationTest
{
    public class RentBookTest : IntegrationTest
    {
        [Fact]
        public async Task RentBookNoAuth()
        {
            //Act
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.UserRoutes.RentBook, new RentBooksRequest());

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RentBookWithAuth()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.UserRoutes.RentBook, new RentBooksRequest());

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            (await DeserializeResponseJson<int>(response.Content)).Should().Be(0);
        }

        class RentBookTestDbContext : DataContext
        {
            public RentBookTestDbContext(DbContextOptions<DataContext> options)
                : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                builder.Entity<Book>().HasData(
                    new Book { Name = "Book 1" },
                    new Book { Name = "Book 2" },
                    new Book { Name = "Book 3" }
                );
            }
        }

        protected DbContextOptions<DataContext> ContextOptions { get; }

        //public RentBookTest() : base()
        //{
        //    var appFactory = new WebApplicationFactory<Startup>()
        //        .WithWebHostBuilder(builder =>
        //        {
        //            builder.ConfigureServices(services =>
        //            {
        //                services.RemoveAll(typeof(DataContext));
        //                services.AddDbContext<RentBookTestDbContext>(options =>
        //                {
        //                    options.UseInMemoryDatabase("InMemoryDbForTesting");
        //                });

        //                services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //                    .AddEntityFrameworkStores<RentBookTestDbContext>();
        //            });
        //        });
        //    _testClient = appFactory.CreateClient();
        //}

        //private void Seed()
        //{
        //    using (var context = new DataContext(ContextOptions))
        //    {
        //        context.Database.EnsureDeleted();
        //        context.Database.EnsureCreated();

        //        var one = new Book { Name = "Book 1" };
        //        var two = new Book { Name = "Book 2" };
        //        var three = new Book { Name = "Book 3" };

        //        context.AddRange(one, two, three);

        //        context.SaveChanges();
        //    }
        //}
    }
}
