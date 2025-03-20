using FluentResults;
using Messaging.Shared;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Application.Repositories;
using Messaging.Shared.Events;

namespace Services.Microservice.EventHandlers
{
    /// <summary>
    /// Обработчик события запроса данных о сервисе
    /// </summary>
    public class ServiceRequestedEventHandler : IIntegrationEventHandler<ServiceRequestedEvent>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IEventBus _eventBus;
        private readonly ILogger<ServiceRequestedEventHandler> _logger;

        public ServiceRequestedEventHandler(
            IServiceRepository serviceRepository,
            IEventBus eventBus,
            ILogger<ServiceRequestedEventHandler> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Обрабатывает запрос данных о сервисе и отправляет ответ
        /// </summary>
        public async Task<Result> HandleAsync(ServiceRequestedEvent @event)
        {
            _logger.LogInformation("Handling ServiceRequestedEvent with correlationId: {@CorrelationId}", @event.CorrelationId);
            
            try
            {
                if (@event.ServiceId.HasValue)
                {
                    // Запрос для конкретного сервиса
                    var serviceResult = await _serviceRepository.GetOneByID(@event.ServiceId.Value);
                    
                    if (serviceResult.IsFailed)
                    {
                        await _eventBus.PublishAsync(new ServiceResponseEvent(@event.CorrelationId, 
                            $"Service with ID {@event.ServiceId.Value} not found"));
                        return Result.Ok();
                    }

                    var service = serviceResult.Value;
                    var serviceDto = MapToServiceDto(service);
                    
                    await _eventBus.PublishAsync(new ServiceResponseEvent(@event.CorrelationId, serviceDto));
                }
                else
                {
                    // Запрос для всех сервисов
                    var servicesResult = await _serviceRepository.GetAll();
                    
                    if (servicesResult.IsFailed)
                    {
                        await _eventBus.PublishAsync(new ServiceResponseEvent(@event.CorrelationId, 
                            "Failed to retrieve services"));
                        return Result.Ok();
                    }

                    var services = servicesResult.Value;
                    var serviceDtos = services.Select(MapToServiceDto).ToList();
                    
                    await _eventBus.PublishAsync(new ServiceResponseEvent(@event.CorrelationId, serviceDtos));
                }
                
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ServiceRequestedEvent");
                
                await _eventBus.PublishAsync(new ServiceResponseEvent(@event.CorrelationId, 
                    $"Error processing request: {ex.Message}"));
                
                return Result.Fail(new Error("Error processing request")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        /// <summary>
        /// Преобразует сущность Service в DTO для передачи через шину событий
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
                IsActive = service.IsActive,
            };
        }
    }

    /// <summary>
    /// Обработчик события создания сервиса в основном приложении
    /// </summary>
    public class ServiceCreatedEventHandler : IIntegrationEventHandler<ServiceCreatedEvent>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<ServiceCreatedEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public ServiceCreatedEventHandler(
            IServiceRepository serviceRepository,
            IEventBus eventBus,
            ILogger<ServiceCreatedEventHandler> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        /// <summary>
        /// Обрабатывает событие создания сервиса
        /// </summary>
        public async Task<Result> HandleAsync(ServiceCreatedEvent @event)
        {
            _logger.LogInformation("Handling ServiceCreatedEvent with serviceId: {@ServiceId}", @event.ServiceId);
            
            try
            {
                // Проверяем, существует ли сервис уже (если это повторное сообщение)
                var existingServiceResult = await _serviceRepository.GetOneByID(@event.ServiceId);
                if (existingServiceResult.IsSuccess)
                {
                    _logger.LogInformation("Service with ID {@ServiceId} already exists, ignoring CreateEvent", @event.ServiceId);
                    return Result.Ok();
                }
                
                // Создаем новый сервис
                var service = new Service
                {
                    Id = @event.ServiceId,
                    Name = @event.Name,
                    ShortName = @event.ShortName,
                    Code = @event.Code,
                    Description = @event.Description,
                    DayCount = @event.DayCount,
                    WorkflowId = @event.WorkflowId,
                    Price = @event.Price,
                    IsActive = @event.IsActive
                };
                
                var result = await _serviceRepository.Add(service);
                
                if (result.IsFailed)
                {
                    _logger.LogError("Failed to create service from event: {Errors}", string.Join(", ", result.Errors.Select(e => e.Message)));
                    return Result.Fail(new Error("Failed to create service from event"));
                }
                

                _logger.LogInformation("Successfully created service with ID {ServiceId} from event", @event.ServiceId);
                
                service.Id = result.Value;
                var serviceDto = MapToServiceDto(service);
                await _eventBus.PublishAsync(new ServiceResponseEvent(@event.CorrelationId, serviceDto));
                
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ServiceCreatedEvent");
                return Result.Fail(new Error("Error processing ServiceCreatedEvent")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }
        
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
                IsActive = service.IsActive,
            };
        }
    }

    /// <summary>
    /// Обработчик события обновления сервиса в основном приложении
    /// </summary>
    public class ServiceUpdatedEventHandler : IIntegrationEventHandler<ServiceUpdatedEvent>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<ServiceUpdatedEventHandler> _logger;

        public ServiceUpdatedEventHandler(
            IServiceRepository serviceRepository,
            ILogger<ServiceUpdatedEventHandler> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Обрабатывает событие обновления сервиса
        /// </summary>
        public async Task<Result> HandleAsync(ServiceUpdatedEvent @event)
        {
            _logger.LogInformation("Handling ServiceUpdatedEvent with serviceId: {@ServiceId}", @event.ServiceId);
            
            try
            {
                // Получаем существующий сервис
                var existingServiceResult = await _serviceRepository.GetOneByID(@event.ServiceId);
                if (existingServiceResult.IsFailed)
                {
                    _logger.LogWarning("Service with ID {@ServiceId} not found for update event", @event.ServiceId);
                    
                    // Создаем сервис, если его нет (idempotent operation)
                    var newService = new Service
                    {
                        Id = @event.ServiceId,
                        Name = @event.Name,
                        ShortName = @event.ShortName,
                        Code = @event.Code,
                        Description = @event.Description,
                        DayCount = @event.DayCount,
                        WorkflowId = @event.WorkflowId,
                        Price = @event.Price,
                        IsActive = @event.IsActive
                    };
                    
                    var createResult = await _serviceRepository.Add(newService);
                    
                    if (createResult.IsFailed)
                    {
                        _logger.LogError("Failed to create service from update event: {Errors}", 
                            string.Join(", ", createResult.Errors.Select(e => e.Message)));
                        return Result.Fail(new Error("Failed to create service from update event"));
                    }
                    
                    _logger.LogInformation("Successfully created service with ID {ServiceId} from update event", @event.ServiceId);
                    return Result.Ok();
                }
                
                // Обновляем существующий сервис
                var existingService = existingServiceResult.Value;
                existingService.Name = @event.Name;
                existingService.ShortName = @event.ShortName;
                existingService.Code = @event.Code;
                existingService.Description = @event.Description;
                existingService.DayCount = @event.DayCount;
                existingService.WorkflowId = @event.WorkflowId;
                existingService.Price = @event.Price;
                existingService.IsActive = @event.IsActive;
                
                var updateResult = await _serviceRepository.Update(existingService);
                
                if (updateResult.IsFailed)
                {
                    _logger.LogError("Failed to update service from event: {Errors}", 
                        string.Join(", ", updateResult.Errors.Select(e => e.Message)));
                    return Result.Fail(new Error("Failed to update service from event"));
                }
                
                _logger.LogInformation("Successfully updated service with ID {ServiceId} from event", @event.ServiceId);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ServiceUpdatedEvent");
                return Result.Fail(new Error("Error processing ServiceUpdatedEvent")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }
    }

    /// <summary>
    /// Обработчик события удаления сервиса в основном приложении
    /// </summary>
    public class ServiceDeletedEventHandler : IIntegrationEventHandler<ServiceDeletedEvent>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger<ServiceDeletedEventHandler> _logger;

        public ServiceDeletedEventHandler(
            IServiceRepository serviceRepository,
            ILogger<ServiceDeletedEventHandler> logger)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Обрабатывает событие удаления сервиса
        /// </summary>
        public async Task<Result> HandleAsync(ServiceDeletedEvent @event)
        {
            _logger.LogInformation("Handling ServiceDeletedEvent with serviceId: {@ServiceId}", @event.ServiceId);
            
            try
            {
                // Проверяем, существует ли сервис
                var existingServiceResult = await _serviceRepository.GetOneByID(@event.ServiceId);
                if (existingServiceResult.IsFailed)
                {
                    _logger.LogWarning("Service with ID {@ServiceId} not found for delete event, ignoring", @event.ServiceId);
                    return Result.Ok();
                }
                
                // Удаляем сервис
                var deleteResult = await _serviceRepository.Delete(@event.ServiceId);
                
                if (deleteResult.IsFailed)
                {
                    _logger.LogError("Failed to delete service from event: {Errors}", 
                        string.Join(", ", deleteResult.Errors.Select(e => e.Message)));
                    return Result.Fail(new Error("Failed to delete service from event"));
                }
                
                _logger.LogInformation("Successfully deleted service with ID {ServiceId} from event", @event.ServiceId);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ServiceDeletedEvent");
                return Result.Fail(new Error("Error processing ServiceDeletedEvent")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }
    }
}