using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EmployeeControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/Employee/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO employee (last_name) 
                    VALUES ('Test last_name') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.GetAsync($"/Employee/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            var requestDto = new
            {
                last_name = "Test",
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Employee/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO employee (last_name) 
                    VALUES ('Test') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            var requestDto = new
            {
                id = id,
                name = "Updated Name",
                description = "Updated Description",
                code = "UPD"
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/Employee/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO employee (last_name) 
                    VALUES ('Test') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.DeleteAsync($"/Employee/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}