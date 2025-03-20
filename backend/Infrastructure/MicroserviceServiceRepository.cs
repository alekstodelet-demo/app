using System.Data;
using Application.Models;
using Application.Repositories;
using Domain.Entities;
using FluentResults;
using Messaging.Shared;
using Messaging.Shared.Events;
using Microsoft.Extensions.Logging;
using WebApi.EventHandlers;

namespace WebApi.Repositories
{
    /// <summary>
    /// Репозиторий для работы с сервисами через микросервис
    /// </summary>
    public class MicroserviceServiceRepository : IServiceRepository
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<MicroserviceServiceRepository> _logger;
        private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);

        public MicroserviceServiceRepository(
            IEventBus eventBus,
            ILogger<MicroserviceServiceRepository> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Получает все сервисы из микросервиса
        /// </summary>
        public async Task<Result<List<Service>>> GetAll()
        {
            _logger.LogInformation("Getting all services from microservice");

            try
            {
                // Создаем уникальный идентификатор для сопоставления запроса и ответа
                var correlationId = Guid.NewGuid();

                // Регистрируем запрос в обработчике ответов
                var responseTask = ServiceResponseEventHandler.RegisterRequest(correlationId, _requestTimeout);

                // Отправляем запрос на получение всех сервисов
                await _eventBus.PublishAsync(new ServiceRequestedEvent(null, correlationId));

                // Ожидаем ответ
                var response = await responseTask;

                if (!response.Success)
                {
                    return Result.Fail(new Error(response.ErrorMessage));
                }

                if (response.Services == null)
                {
                    return Result.Fail(new Error("Invalid response from service microservice"));
                }

                // Преобразуем DTO в доменные объекты
                var services = response.Services.Select(MapToService).ToList();

                return Result.Ok(services);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout while getting all services from microservice");
                return Result.Fail(new Error("Timeout while getting all services from microservice"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all services from microservice");
                return Result.Fail(new Error("Error getting all services from microservice")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        /// <summary>
        /// Получает сервис по идентификатору из микросервиса
        /// </summary>
        public async Task<Result<Service>> GetOneByID(int id)
        {
            _logger.LogInformation("Getting service with ID {ServiceId} from microservice", id);

            try
            {
                // Создаем уникальный идентификатор для сопоставления запроса и ответа
                var correlationId = Guid.NewGuid();

                // Регистрируем запрос в обработчике ответов
                var responseTask = ServiceResponseEventHandler.RegisterRequest(correlationId, _requestTimeout);

                // Отправляем запрос на получение сервиса по ID
                await _eventBus.PublishAsync(new ServiceRequestedEvent(id, correlationId));
                
                // Ожидаем ответ
                var response = await responseTask;

                if (!response.Success)
                {
                    return Result.Fail(new Error(response.ErrorMessage));
                }

                if (response.Service == null)
                {
                    return Result.Fail(new Error($"Service with ID {id} not found"));
                }

                // Преобразуем DTO в доменный объект
                var service = MapToService(response.Service);

                return Result.Ok(service);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout while getting service with ID {ServiceId} from microservice", id);
                return Result.Fail(new Error($"Timeout while getting service with ID {id} from microservice"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting service with ID {ServiceId} from microservice", id);
                return Result.Fail(new Error($"Error getting service with ID {id} from microservice")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        /// <summary>
        /// Добавляет новый сервис через микросервис
        /// </summary>
        public async Task<Result<int>> Add(Service entity)
        {
            _logger.LogInformation("Adding new service through microservice");

            try
            {
                var correlationId = Guid.NewGuid();
                
                var responseTask = ServiceResponseEventHandler.RegisterRequest(correlationId, _requestTimeout);
                
                // Публикуем событие создания сервиса
                await _eventBus.PublishAsync(new ServiceCreatedEvent(
                    entity.Id,
                    entity.Name,
                    entity.ShortName,
                    entity.Code,
                    entity.Description,
                    entity.DayCount,
                    entity.WorkflowId,
                    entity.Price,
                    entity.IsActive,
                    correlationId));

                // Ожидаем ответ
                var response = await responseTask;

                if (!response.Success)
                {
                    return Result.Fail(new Error(response.ErrorMessage));
                }

                if (response.Service == null)
                {
                    return Result.Fail(new Error($"Error adding service through microservice"));
                }

                // Преобразуем DTO в доменный объект
                var service = MapToService(response.Service);

                return Result.Ok(service.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding service through microservice");
                return Result.Fail(new Error("Error adding service through microservice")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        /// <summary>
        /// Обновляет сервис через микросервис
        /// </summary>
        public async Task<Result> Update(Service entity)
        {
            _logger.LogInformation("Updating service with ID {ServiceId} through microservice", entity.Id);

            try
            {
                var correlationId = Guid.NewGuid();
                
                var responseTask = ServiceResponseEventHandler.RegisterRequest(correlationId, _requestTimeout);
                
                // Публикуем событие обновления сервиса
                await _eventBus.PublishAsync(new ServiceUpdatedEvent(
                    entity.Id,
                    entity.Name,
                    entity.ShortName,
                    entity.Code,
                    entity.Description,
                    entity.DayCount,
                    entity.WorkflowId,
                    entity.Price,
                    entity.IsActive,
                    correlationId));
                
                // Ожидаем ответ
                var response = await responseTask;

                if (!response.Success)
                {
                    return Result.Fail(new Error(response.ErrorMessage));
                }

                if (response.Service == null)
                {
                    return Result.Fail(new Error($"Error updating service through microservice"));
                }

                // Преобразуем DTO в доменный объект
                var service = MapToService(response.Service);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service with ID {ServiceId} through microservice", entity.Id);
                return Result.Fail(new Error($"Error updating service with ID {entity.Id} through microservice")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        /// <summary>
        /// Получает пагинированный список сервисов из микросервиса
        /// </summary>
        public async Task<Result<PaginatedList<Service>>> GetPaginated(int pageSize, int pageNumber)
        {
            _logger.LogInformation(
                "Getting paginated services from microservice (page {PageNumber}, size {PageSize})",
                pageNumber, pageSize);

            try
            {
                // В текущей реализации микросервиса нет поддержки пагинации,
                // поэтому мы получаем все сервисы и делаем пагинацию на клиенте
                var result = await GetAll();

                if (result.IsFailed)
                {
                    return Result.Fail(result.Errors);
                }

                var services = result.Value;

                // Применяем пагинацию
                var totalCount = services.Count;
                var items = services
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Result.Ok(new PaginatedList<Service>(items, totalCount, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated services from microservice");
                return Result.Fail(new Error("Error getting paginated services from microservice")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        /// <summary>
        /// Удаляет сервис через микросервис
        /// </summary>
        public async Task<Result> Delete(int id)
        {
            _logger.LogInformation("Deleting service with ID {ServiceId} through microservice", id);

            try
            {
                // Публикуем событие удаления сервиса
                await _eventBus.PublishAsync(new ServiceDeletedEvent(id));

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service with ID {ServiceId} through microservice", id);
                return Result.Fail(new Error($"Error deleting service with ID {id} through microservice")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Преобразует DTO в доменный объект
        /// </summary>
        private Service MapToService(ServiceDto dto)
        {
            return new Service
            {
                Id = dto.Id,
                Name = dto.Name,
                ShortName = dto.ShortName,
                Code = dto.Code,
                Description = dto.Description,
                DayCount = dto.DayCount,
                WorkflowId = dto.WorkflowId,
                Price = dto.Price,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                CreatedBy = dto.CreatedBy,
                UpdatedBy = dto.UpdatedBy
            };
        }
    }
}