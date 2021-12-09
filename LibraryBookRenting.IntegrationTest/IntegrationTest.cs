using LibraryBookRenting.Contracts;
using LibraryBookRenting.Contracts.Requests;
using LibraryBookRenting.Contracts.Responses;
using LibraryBookRenting.Data;
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

namespace LibraryBookRenting.IntegrationTest
{
    public class IntegrationTest
    {
        protected HttpClient _testClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });
                    });
                });
            _testClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            _testClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var requestAuth = new CreateUserRequest
            {
                UserName = "truongTestrrt",
                Password = "TruongTest@123",
            };
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.UserRoutes.Create, requestAuth);
            var authenticationResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

            if (!authenticationResponse.IsSuccess)
            {
                response = await _testClient.PostAsJsonAsync(ApiRoutes.UserRoutes.Signin, requestAuth);
                authenticationResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
            }
            return authenticationResponse.Token;
        }

        protected async Task<T> DeserializeResponseJson<T>(System.Net.Http.HttpContent content)
        {
            var contentStream = await content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();
            return serializer.Deserialize<T>(jsonReader);
        }
    }
}
