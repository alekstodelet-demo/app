using Application.Repositories;
using Domain.Entities;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Microservice.Dtos;

namespace Services.Microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(
            IServiceRepository serviceRepository,
            ILogger<ServiceController> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all services");
            
            try
            {
                var result = await _serviceRepository.GetAll();
                
                if (result.IsFailed)
                {
                    _logger.LogWarning("Failed to get all services: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Message)));
                    return BadRequest(new { Errors = result.Errors.Select(e => e.Message).ToList() });
                }
                
                var services = result.Value;
                var serviceDtos = services.Select(s => MapToServiceDto(s)).ToList();
                
                return Ok(serviceDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all services");
                return StatusCode(500, new { Error = "An error occurred while retrieving services" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Getting service with ID {ServiceId}", id);
            
            try
            {
                var result = await _serviceRepository.GetOneByID(id);
                
                if (result.IsFailed)
                {
                    _logger.LogWarning("Failed to get service with ID {ServiceId}: {Errors}", 
                        id, string.Join(", ", result.Errors.Select(e => e.Message)));
                    return NotFound(new { Error = $"Service with ID {id} not found" });
                }
                
                var service = result.Value;
                var serviceDto = MapToServiceDto(service);
                
                return Ok(serviceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting service with ID {ServiceId}", id);
                return StatusCode(500, new { Error = $"An error occurred while retrieving service with ID {id}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateServiceRequest request)
        {
            _logger.LogInformation("Creating new service");
            
            try
            {
                var service = new Service
                {
                    Name = request.Name,
                    ShortName = request.ShortName,
                    Code = request.Code,
                    Description = request.Description,
                    DayCount = request.DayCount,
                    WorkflowId = request.WorkflowId,
                    Price = request.Price,
                    IsActive = request.IsActive
                };
                
                var result = await _serviceRepository.Add(service);
                
                if (result.IsFailed)
                {
                    _logger.LogWarning("Failed to create service: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Message)));
                    return BadRequest(new { Errors = result.Errors.Select(e => e.Message).ToList() });
                }
                
                var id = result.Value;
                service.Id = id;
                
                return CreatedAtAction(nameof(GetById), new { id }, MapToServiceDto(service));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating service");
                return StatusCode(500, new { Error = "An error occurred while creating the service" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceRequest request)
        {
            _logger.LogInformation("Updating service with ID {ServiceId}", id);
            
            try
            {
                // Проверяем, что ID в URL и в теле запроса совпадают
                if (id != request.Id)
                {
                    return BadRequest(new { Error = "ID in URL does not match ID in request body" });
                }
                
                // Проверяем, что сервис существует
                var existingResult = await _serviceRepository.GetOneByID(id);
                
                if (existingResult.IsFailed)
                {
                    _logger.LogWarning("Failed to find service with ID {ServiceId} for update", id);
                    return NotFound(new { Error = $"Service with ID {id} not found" });
                }
                
                var service = new Service
                {
                    Id = request.Id,
                    Name = request.Name,
                    ShortName = request.ShortName,
                    Code = request.Code,
                    Description = request.Description,
                    DayCount = request.DayCount,
                    WorkflowId = request.WorkflowId,
                    Price = request.Price,
                    IsActive = request.IsActive
                };
                
                var result = await _serviceRepository.Update(service);
                
                if (result.IsFailed)
                {
                    _logger.LogWarning("Failed to update service with ID {ServiceId}: {Errors}", 
                        id, string.Join(", ", result.Errors.Select(e => e.Message)));
                    return BadRequest(new { Errors = result.Errors.Select(e => e.Message).ToList() });
                }
                
                return Ok(MapToServiceDto(service));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service with ID {ServiceId}", id);
                return StatusCode(500, new { Error = $"An error occurred while updating service with ID {id}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting service with ID {ServiceId}", id);
            
            try
            {
                // Проверяем, что сервис существует
                var existingResult = await _serviceRepository.GetOneByID(id);
                
                if (existingResult.IsFailed)
                {
                    _logger.LogWarning("Failed to find service with ID {ServiceId} for deletion", id);
                    return NotFound(new { Error = $"Service with ID {id} not found" });
                }
                
                var result = await _serviceRepository.Delete(id);
                
                if (result.IsFailed)
                {
                    _logger.LogWarning("Failed to delete service with ID {ServiceId}: {Errors}", 
                        id, string.Join(", ", result.Errors.Select(e => e.Message)));
                    return BadRequest(new { Errors = result.Errors.Select(e => e.Message).ToList() });
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service with ID {ServiceId}", id);
                return StatusCode(500, new { Error = $"An error occurred while deleting service with ID {id}" });
            }
        }

        /// <summary>
        /// Преобразует сущность сервиса в DTO
        /// </summary>
        private ServiceDto MapToServiceDto(Service service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                ShortName = service.ShortName,
                Code = service.Code,
                Description = service.Description,
                DayCount = service.DayCount,
                WorkflowId = service.WorkflowId,
                Price = service.Price,
                IsActive = service.IsActive
            };
        }
    }
}