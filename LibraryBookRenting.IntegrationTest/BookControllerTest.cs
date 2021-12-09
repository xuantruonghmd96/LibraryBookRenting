using FluentAssertions;
using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryBookRenting.IntegrationTest
{
    public class BookControllerTest : IntegrationTest
    {
        //[Fact]
        //public async Task GetAll_WithEmpty()
        //{
        //    //Arrange
        //    await AuthenticateAsync();

        //    //Act
        //    var response = await _testClient.GetAsync(ApiRoutes.BookRoutes.GetAll);

        //    //Assert
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //    (await DeserializeResponseJson<List<BookResponse>>(response.Content)).Count.Should().Be(4);
        //}
    }
}
