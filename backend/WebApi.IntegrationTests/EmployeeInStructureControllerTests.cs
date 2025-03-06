using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class EmployeeInStructureControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public EmployeeInStructureControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/EmployeeInStructure/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            int idEmployee = DatabaseHelper.RunQuery(@"INSERT INTO employee (last_name) VALUES ('test employee') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('test org_structure') RETURNING id;");
            var id = DatabaseHelper.RunQuery(@"
                            INSERT INTO employee_in_structure (employee_id, structure_id, date_start) 
                            VALUES (@idEmployee, @idStructure, @dateStart) 
                            RETURNING id;", 
                new Dictionary<string, object>
                {
                    ["@idEmployee"] = idEmployee,
                    ["@idStructure"] = idStructure,
                    ["@dateStart"] = DateTime.Now
                });

            // Act
            var response = await _client.GetAsync($"/EmployeeInStructure/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            int idEmployee = DatabaseHelper.RunQuery(@"INSERT INTO employee (last_name) VALUES ('test employee') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('test org_structure') RETURNING id;");
            var requestDto = new
            {
                employee_id = idEmployee,
                structure_id = idStructure,
                date_start = DateTime.Now
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/EmployeeInStructure/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            int idEmployee = DatabaseHelper.RunQuery(@"INSERT INTO employee (last_name) VALUES ('test employee') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('test org_structure') RETURNING id;");
            var id = DatabaseHelper.RunQuery(@"
                            INSERT INTO employee_in_structure (employee_id, structure_id, date_start) 
                            VALUES (@idEmployee, @idStructure, @dateStart) 
                            RETURNING id;", 
                new Dictionary<string, object>
            {
                ["@idEmployee"] = idEmployee,
                ["@idStructure"] = idStructure,
                ["@dateStart"] = DateTime.Now
            });

            var requestDto = new
            {
                id = id,
                employee_id = idEmployee,
                structure_id = idStructure,
                date_start = DateTime.Now
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/EmployeeInStructure/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            int idEmployee = DatabaseHelper.RunQuery(@"INSERT INTO employee (last_name) VALUES ('test employee') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('test org_structure') RETURNING id;");
            var id = DatabaseHelper.RunQuery(@"
                            INSERT INTO employee_in_structure (employee_id, structure_id, date_start) 
                            VALUES (@idEmployee, @idStructure, @dateStart) 
                            RETURNING id;", 
                new Dictionary<string, object>
                {
                    ["@idEmployee"] = idEmployee,
                    ["@idStructure"] = idStructure,
                    ["@dateStart"] = DateTime.Now
                });

            // Act
            var response = await _client.DeleteAsync($"/EmployeeInStructure/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}