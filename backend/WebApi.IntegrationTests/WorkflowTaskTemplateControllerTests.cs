using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class WorkflowTaskTemplateControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public WorkflowTaskTemplateControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/WorkflowTaskTemplate/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO workflow_task_template (name) 
                    VALUES ('Test') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.GetAsync($"/WorkflowTaskTemplate/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            var requestDto = new
            {
                name = "Test",
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/WorkflowTaskTemplate/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO workflow_task_template (name) 
                    VALUES ('Test') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            var requestDto = new
            {
                id = id,
                name = "Updated Name",
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/WorkflowTaskTemplate/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO workflow_task_template (name) 
                    VALUES ('Test') 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.DeleteAsync($"/WorkflowTaskTemplate/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}