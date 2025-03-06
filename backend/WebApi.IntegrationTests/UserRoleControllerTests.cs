using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Text;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests
{
    public class UserRoleControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserRoleControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DatabaseHelper.ClearTables();
        }

        [Fact]
        public async Task GetAll_ReturnsOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/UserRole/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOneById_ReturnsOkResponse()
        {
            // Arrange
            int idRole = DatabaseHelper.RunQuery(@"INSERT INTO ""Role"" (name) VALUES ('Test role') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('Test structure') RETURNING id;");
            string query = @$"INSERT INTO user_role (role_id, structure_id) VALUES ({idRole}, {idStructure}) RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.GetAsync($"/UserRole/GetOneById?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_ReturnsOkResponse()
        {
            // Arrange
            int idRole = DatabaseHelper.RunQuery(@"INSERT INTO ""Role"" (name) VALUES ('Test role') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('Test structure') RETURNING id;");
            int idUser = DatabaseHelper.RunQuery(@"INSERT INTO ""User"" (email, userid, password_hash, first_reset) VALUES ('login', '0001', 'pass', true) RETURNING id;");
            var requestDto = new
            {
                role_id = idRole,
                structure_id = idStructure,
                user_id = idUser,
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/UserRole/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ReturnsOkResponse()
        {
            // Arrange
            int idRole = DatabaseHelper.RunQuery(@"INSERT INTO ""Role"" (name) VALUES ('Test role') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('Test structure') RETURNING id;");
            int idStructureSecond = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('Test structure second') RETURNING id;");
            int idUser = DatabaseHelper.RunQuery(@"INSERT INTO ""User"" (email, userid, password_hash, first_reset) VALUES ('login', '0001', 'pass', true) RETURNING id;");
            string query = @$"INSERT INTO user_role (role_id, structure_id) VALUES ({idRole}, {idStructure}) RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            var requestDto = new
            {
                id = id,
                role_id = idRole,
                structure_id = idStructureSecond,
                user_id = idUser,
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/UserRole/Update", content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete_ReturnsOkResponse()
        {
            // Arrange
            int idRole = DatabaseHelper.RunQuery(@"INSERT INTO ""Role"" (name) VALUES ('Test role') RETURNING id;");
            int idStructure = DatabaseHelper.RunQuery(@"INSERT INTO org_structure (name) VALUES ('Test structure') RETURNING id;");
            string query = @$"INSERT INTO user_role (role_id, structure_id) VALUES ({idRole}, {idStructure}) RETURNING id;";
            int id = DatabaseHelper.RunQuery(query);

            // Act
            var response = await _client.DeleteAsync($"/UserRole/Delete?id={id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}