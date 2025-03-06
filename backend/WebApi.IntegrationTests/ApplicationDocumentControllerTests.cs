using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class ApplicationDocumentControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ApplicationDocumentControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/ApplicationDocument/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            string document_type = @"
                    INSERT INTO application_document_type (name) 
                    VALUES ('Test Document Type') 
                    RETURNING id;";
            int idType = DatabaseHelper.RunQuery(document_type);

            string query = @$"
                    INSERT INTO application_document (name, document_type_id) 
                    VALUES ('Test Document', {idType}) 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.GetAsync($"/ApplicationDocument/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            string document_type = @"
                    INSERT INTO application_document_type (name) 
                    VALUES ('Test Document Type') 
                    RETURNING id;";
            int idType = DatabaseHelper.RunQuery(document_type);

            var requestDto = new
            {
                name = "Test Document",
                document_type_id = idType,
                description = "TST",
                law_description = "test"
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/ApplicationDocument/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            string document_type = @"
                    INSERT INTO application_document_type (name) 
                    VALUES ('Test Document Type') 
                    RETURNING id;";
            int idType = DatabaseHelper.RunQuery(document_type);

            string query = @$"
                    INSERT INTO application_document (name, document_type_id) 
                    VALUES ('Test Document', {idType}) 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            var requestDto = new
            {
                id = id,
                name = "Updated Name",
                document_type_id = idType,
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/ApplicationDocument/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            string document_type = @"
                    INSERT INTO application_document_type (name) 
                    VALUES ('Test Document Type') 
                    RETURNING id;";
            int idType = DatabaseHelper.RunQuery(document_type);

            string query = @$"
                    INSERT INTO application_document (name, document_type_id) 
                    VALUES ('Test Document', {idType}) 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.DeleteAsync($"/ApplicationDocument/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}