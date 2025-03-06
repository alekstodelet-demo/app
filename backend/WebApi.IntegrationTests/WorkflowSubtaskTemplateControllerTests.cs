using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class WorkflowSubtaskTemplateControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public WorkflowSubtaskTemplateControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/WorkflowSubtaskTemplate/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO workflow_subtask_template (name, workflow_task_id) 
                    VALUES ('Test', 0) 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.GetAsync($"/WorkflowSubtaskTemplate/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            var requestDto = new
            {
                name = "Name",
                workflow_task_id = 0
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/WorkflowSubtaskTemplate/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO workflow_subtask_template (name, workflow_task_id) 
                    VALUES ('Test', 0) 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            var requestDto = new
            {
                id = id,
                name = "Updated Name",
                workflow_task_id = 0
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/WorkflowSubtaskTemplate/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            string query = @"
                    INSERT INTO workflow_subtask_template (name, workflow_task_id) 
                    VALUES ('Test', 0) 
                    RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.DeleteAsync($"/WorkflowSubtaskTemplate/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}