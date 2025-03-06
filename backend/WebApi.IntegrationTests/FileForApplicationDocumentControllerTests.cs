using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class FileForApplicationDocumentControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FileForApplicationDocumentControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/FileForApplicationDocument/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            int idFile = DatabaseHelper.RunQuery(@"INSERT INTO file (name) VALUES ('TestFile') RETURNING id;");
            int idFileType = DatabaseHelper.RunQuery(@"INSERT INTO file_type_for_application_document (name) VALUES ('Test') RETURNING id;");
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            int id = DatabaseHelper.RunQuery(@$"INSERT INTO file_for_application_document (name, file_id, document_id, type_id) 
                    VALUES ('TestDocument', {idFile}, {idADocument}, {idFileType}) RETURNING id;");

            // Act
            var response = await _client.GetAsync($"/FileForApplicationDocument/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            int idFile = DatabaseHelper.RunQuery(@"INSERT INTO file (name) VALUES ('TestFile') RETURNING id;");
            int idFileType = DatabaseHelper.RunQuery(@"INSERT INTO file_type_for_application_document (name) VALUES ('Test') RETURNING id;");
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            var requestDto = new
            {
                name = "Test Document",
                file_id = idFile,
                document_id = idADocument,
                type_id = idFileType
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/FileForApplicationDocument/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            int idFile = DatabaseHelper.RunQuery(@"INSERT INTO file (name) VALUES ('TestFile') RETURNING id;");
            int idFileType = DatabaseHelper.RunQuery(@"INSERT INTO file_type_for_application_document (name) VALUES ('Test') RETURNING id;");
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            int id = DatabaseHelper.RunQuery(@$"INSERT INTO file_for_application_document (name, file_id, document_id, type_id) 
                    VALUES ('TestDocument', {idFile}, {idADocument}, {idFileType}) RETURNING id;");
            
            var requestDto = new
            {
                id = id,
                name = "Updated Name",
                file_id = idFile,
                document_id = idADocument,
                type_id = idFileType
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/FileForApplicationDocument/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            int idFile = DatabaseHelper.RunQuery(@"INSERT INTO file (name) VALUES ('TestFile') RETURNING id;");
            int idFileType = DatabaseHelper.RunQuery(@"INSERT INTO file_type_for_application_document (name) VALUES ('Test') RETURNING id;");
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            int id = DatabaseHelper.RunQuery(@$"INSERT INTO file_for_application_document (name, file_id, document_id, type_id) 
                    VALUES ('TestDocument', {idFile}, {idADocument}, {idFileType}) RETURNING id;");

            // Act
            var response = await _client.DeleteAsync($"/FileForApplicationDocument/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}