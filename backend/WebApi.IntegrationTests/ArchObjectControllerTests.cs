using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class ArchObjectControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ArchObjectControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/ArchObject/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            int id = DatabaseHelper.RunQuery(@"
                    INSERT INTO arch_object (address) 
                    VALUES ('Test') 
                    RETURNING id;");
            // Act
            var response = await _client.GetAsync($"/ArchObject/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            var requestDto = new
            {
                address = "Test"
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/ArchObject/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            int id = DatabaseHelper.RunQuery(@"
                    INSERT INTO arch_object (address) 
                    VALUES ('Test') 
                    RETURNING id;");

            var requestDto = new
            {
                id = id,
                address = "update name",
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/ArchObject/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            int id = DatabaseHelper.RunQuery(@"
                    INSERT INTO arch_object (address) 
                    VALUES ('Test') 
                    RETURNING id;");

            // Act
            var response = await _client.DeleteAsync($"/ArchObject/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}