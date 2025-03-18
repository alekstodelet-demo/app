using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using WebApi.Controllers;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.IntegrationTests.ControllerTests
{
    public class ServiceControllerTests
    {
        private readonly Mock<IServiceRepository> _mockRepo;
        private readonly ServiceController _controller;

        public ServiceControllerTests()
        {
            // Setup that runs before each test
            _mockRepo = new Mock<IServiceRepository>();
            _controller = new ServiceController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllServices()
        {
            // Arrange
            var testServices = GetTestServices();
            _mockRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(testServices);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var returnedServices = Assert.IsType<List<Service>>(okResult.Value);
            Assert.Equal(3, returnedServices.Count);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsService()
        {
            // Arrange
            var testService = new Service
            {
                Id = 1,
                Name = "Test Service",
                Short_name = "Test",
                Code = "TEST001",
                Description = "Test service description",
                Day_count = 5,
                Price = 1000,
                Workflow_id = 2
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(testService);

            // Act
            var result = await _controller.GetOneById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var returnedService = Assert.IsType<Service>(okResult.Value);
            Assert.Equal(1, returnedService.Id);
            Assert.Equal("Test Service", returnedService.Name);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Service)null);

            // Act
            var result = await _controller.GetOneById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_WithValidService_ReturnsCreatedService()
        {
            // Arrange
            var newService = new Service
            {
                Name = "New Service",
                Short_name = "New",
                Code = "NEW001",
                Description = "New service description",
                Day_count = 7,
                Price = 1500,
                Workflow_id = 3
            };

            var createdService = new Service
            {
                Id = 4,
                Name = "New Service",
                Short_name = "New",
                Code = "NEW001",
                Description = "New service description",
                Day_count = 7,
                Price = 1500,
                Workflow_id = 3
            };

            _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Service>()))
                .ReturnsAsync(createdService);

            // Act
            var result = await _controller.Create(newService);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);

            var returnedService = Assert.IsType<Service>(createdResult.Value);
            Assert.Equal(4, returnedService.Id);
            Assert.Equal("New Service", returnedService.Name);
        }

        [Fact]
        public async Task Create_WithInvalidService_ReturnsBadRequest()
        {
            // Arrange
            var invalidService = new Service(); // Empty service with no required fields
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(invalidService);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_WithValidService_ReturnsUpdatedService()
        {
            // Arrange
            var serviceToUpdate = new Service
            {
                Id = 2,
                Name = "Updated Service",
                Short_name = "Updated",
                Code = "UPD001",
                Description = "Updated service description",
                Day_count = 10,
                Price = 2000,
                Workflow_id = 1
            };

            _mockRepo.Setup(repo => repo.GetByIdAsync(2))
                .ReturnsAsync(new Service { Id = 2 });

            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Service>()))
                .ReturnsAsync(serviceToUpdate);

            // Act
            var result = await _controller.Update(serviceToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var returnedService = Assert.IsType<Service>(okResult.Value);
            Assert.Equal(2, returnedService.Id);
            Assert.Equal("Updated Service", returnedService.Name);
        }

        [Fact]
        public async Task Update_WithNonExistentService_ReturnsNotFound()
        {
            // Arrange
            var nonExistentService = new Service { Id = 999 };

            _mockRepo.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Service)null);

            // Act
            var result = await _controller.Update(nonExistentService);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsOk()
        {
            // Arrange
            var serviceToDelete = new Service { Id = 3 };

            _mockRepo.Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(serviceToDelete);

            _mockRepo.Setup(repo => repo.DeleteAsync(3))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(3);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Delete_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Service)null);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_WithErrorDuringDeletion_ReturnsBadRequest()
        {
            // Arrange
            var serviceToDelete = new Service { Id = 5 };

            _mockRepo.Setup(repo => repo.GetByIdAsync(5))
                .ReturnsAsync(serviceToDelete);

            _mockRepo.Setup(repo => repo.DeleteAsync(5))
                .ReturnsAsync(false); // Deletion failed

            // Act
            var result = await _controller.Delete(5);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private List<Service> GetTestServices()
        {
            return new List<Service>
            {
                new Service
                {
                    Id = 1,
                    Name = "Service 1",
                    Short_name = "S1",
                    Code = "S001",
                    Description = "Description for Service 1",
                    Day_count = 5,
                    Price = 1000,
                    Workflow_id = 1
                },
                new Service
                {
                    Id = 2,
                    Name = "Service 2",
                    Short_name = "S2",
                    Code = "S002",
                    Description = "Description for Service 2",
                    Day_count = 3,
                    Price = 750,
                    Workflow_id = 2
                },
                new Service
                {
                    Id = 3,
                    Name = "Service 3",
                    Short_name = "S3",
                    Code = "S003",
                    Description = "Description for Service 3",
                    Day_count = 7,
                    Price = 1500,
                    Workflow_id = 1
                }
            };
        }
    }

    // Assumed model class based on frontend code
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Short_name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Day_count { get; set; }
        public decimal Price { get; set; }
        public int Workflow_id { get; set; }
    }

    // Interface for the service repository
    public interface IServiceRepository
    {
        Task<List<Service>> GetAllAsync();
        Task<Service> GetByIdAsync(int id);
        Task<Service> CreateAsync(Service service);
        Task<Service> UpdateAsync(Service service);
        Task<bool> DeleteAsync(int id);
    }

    // Controller implementation to test
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var services = await _serviceRepository.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("GetOneById")]
        public async Task<IActionResult> GetOneById(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdService = await _serviceRepository.CreateAsync(service);
            return CreatedAtAction(nameof(GetOneById), new { id = createdService.Id }, createdService);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Service service)
        {
            var existingService = await _serviceRepository.GetByIdAsync(service.Id);
            if (existingService == null)
                return NotFound();

            var updatedService = await _serviceRepository.UpdateAsync(service);
            return Ok(updatedService);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            var result = await _serviceRepository.DeleteAsync(id);
            if (!result)
                return BadRequest("Failed to delete the service");

            return Ok(result);
        }
    }
}