using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class ApplicationDocumentTypeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApplicationDocumentTypeControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/ApplicationDocumentType/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO application_document_type (name) 
                    VALUES ('Test Document Type') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.GetAsync($"/ApplicationDocumentType/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            var requestDto = new
            {
                name = "Test Document Type",
                description = "Test Description",
                code = "TST"
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/ApplicationDocumentType/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO application_document_type (name) 
                    VALUES ('Test Document Type') 
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
            var response = await _client.PutAsync("/ApplicationDocumentType/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO application_document_type (name) 
                    VALUES ('Test Document Type') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.DeleteAsync($"/ApplicationDocumentType/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}