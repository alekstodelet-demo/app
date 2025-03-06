using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class ServiceDocumentControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ServiceDocumentControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/ServiceDocument/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            int idService = DatabaseHelper.RunQuery(@"INSERT INTO service (name) VALUES ('Test Service') RETURNING id;");
            int id = DatabaseHelper.RunQuery(@$"INSERT INTO service_document (service_id, application_document_id) VALUES ({idService}, {idADocument}) RETURNING id;");

            // Act
            var response = await _client.GetAsync($"/ServiceDocument/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            int idService = DatabaseHelper.RunQuery(@"INSERT INTO service (name) VALUES ('Test Service') RETURNING id;");
            var requestDto = new
            {
                service_id = idService,
                application_document_id = idADocument
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/ServiceDocument/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            int idService = DatabaseHelper.RunQuery(@"INSERT INTO service (name) VALUES ('Test Service') RETURNING id;");
            int id = DatabaseHelper.RunQuery(@$"INSERT INTO service_document (service_id, application_document_id) VALUES ({idService}, {idADocument}) RETURNING id;");

            var requestDto = new
            {
                id = id,
                service_id = idService,
                application_document_id = idADocument,
                is_required = true
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/ServiceDocument/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            int idDType = DatabaseHelper.RunQuery(@"INSERT INTO application_document_type (name) VALUES ('TestFile') RETURNING id;");
            int idADocument = DatabaseHelper.RunQuery(@$"INSERT INTO application_document (name, document_type_id) VALUES ('TestFile', {idDType}) RETURNING id;");
            int idService = DatabaseHelper.RunQuery(@"INSERT INTO service (name) VALUES ('Test Service') RETURNING id;");
            int id = DatabaseHelper.RunQuery(@$"INSERT INTO service_document (service_id, application_document_id) VALUES ({idService}, {idADocument}) RETURNING id;");

            // Act
            var response = await _client.DeleteAsync($"/ServiceDocument/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}