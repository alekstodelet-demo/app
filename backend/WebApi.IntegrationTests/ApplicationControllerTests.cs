using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class ApplicationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApplicationControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/Application/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            int id = DatabaseHelper.RunQuery(@"
                    INSERT INTO application (registration_date) 
                    VALUES (@registration_date) 
                    RETURNING id;", new Dictionary<string, object>
                {["@registration_date"] = DateTime.Now});
            // Act
            var response = await _client.GetAsync($"/Application/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            var requestDto = new
            {
                registration_date = DateTime.Now
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Application/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            int id = DatabaseHelper.RunQuery(@"
                    INSERT INTO application (registration_date) 
                    VALUES (@registration_date) 
                    RETURNING id;", new Dictionary<string, object>
                {["@registration_date"] = DateTime.Now});

            var requestDto = new
            {
                id = id,
                registration_date = DateTime.Now,
                deadline = DateTime.Now,
                
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/Application/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            int id = DatabaseHelper.RunQuery(@"
                    INSERT INTO application (registration_date) 
                    VALUES (@registration_date) 
                    RETURNING id;", new Dictionary<string, object>
                {["@registration_date"] = DateTime.Now});

            // Act
            var response = await _client.DeleteAsync($"/Application/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}