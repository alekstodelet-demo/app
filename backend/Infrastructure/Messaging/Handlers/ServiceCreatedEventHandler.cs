using Infrastructure.Messaging.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Application.Repositories;
using Infrastructure.Messaging;

namespace Infrastructure.Messaging.Handlers
{
    /// <summary>
    /// Обработчик события создания сервиса
    /// </summary>
    public class ServiceCreatedEventHandler : IEventHandler<ServiceCreatedEvent>
    {
        private readonly ILogger<ServiceCreatedEventHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceCreatedEventHandler(
            ILogger<ServiceCreatedEventHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ServiceCreatedEvent @event)
        {
            _logger.LogInformation("Handling ServiceCreatedEvent: {ServiceId}", @event.ServiceId);

            // В реальной микросервисной архитектуре тут было бы обновление локальной копии сервиса
            // Здесь это не требуется, так как все в одной базе
            
            // Но можно добавить дополнительную логику, например, уведомления
            _logger.LogInformation("Service created: {ServiceName} (ID: {ServiceId})", @event.Name, @event.ServiceId);
            
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Обработчик события обновления сервиса
    /// </summary>
    public class ServiceUpdatedEventHandler : IEventHandler<ServiceUpdatedEvent>
    {
        private readonly ILogger<ServiceUpdatedEventHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceUpdatedEventHandler(
            ILogger<ServiceUpdatedEventHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ServiceUpdatedEvent @event)
        {
            _logger.LogInformation("Handling ServiceUpdatedEvent: {ServiceId}", @event.ServiceId);

            // В микросервисной архитектуре тут было бы обновление локальной копии сервиса
            // в зависимых сервисах

            // Дополнительная логика обработки обновления сервиса
            _logger.LogInformation("Service updated: {ServiceName} (ID: {ServiceId})", @event.Name, @event.ServiceId);
            
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Обработчик события создания заявки
    /// </summary>
    public class ApplicationCreatedEventHandler : IEventHandler<ApplicationCreatedEvent>
    {
        private readonly ILogger<ApplicationCreatedEventHandler> _logger;
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationCreatedEventHandler(
            ILogger<ApplicationCreatedEventHandler> logger,
            IEventBus eventBus,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _eventBus = eventBus;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ApplicationCreatedEvent @event)
        {
            _logger.LogInformation("Handling ApplicationCreatedEvent: {ApplicationId}", @event.ApplicationId);

            try
            {
                // Если дедлайн не указан, автоматически рассчитываем его
                if (!@event.Deadline.HasValue)
                {
                    // В реальном микросервисном приложении тут был бы запрос к сервису Services
                    // для расчета дедлайна
                    var serviceResult = await _unitOfWork.ServiceRepository.GetOneByID(@event.ServiceId);
                    if (serviceResult.IsSuccess)
                    {
                        var service = serviceResult.Value;
                        
                        // Расчет дедлайна исходя из количества дней в сервисе
                        var calculatedDeadline = DateTime.Now.AddDays(service.DayCount ?? 0);
                        
                        // В реальном приложении тут был бы вызов микросервиса Calendar
                        // для исключения выходных и праздников
                        
                        // Обновляем дедлайн в заявке
                        var applicationResult = await _unitOfWork.ApplicationRepository.GetOneByID(@event.ApplicationId);
                        if (applicationResult.IsSuccess)
                        {
                            var application = applicationResult.Value;
                            application.Deadline = calculatedDeadline;
                            
                            await _unitOfWork.ApplicationRepository.Update(application);
                            _unitOfWork.Commit();
                            
                            // Публикуем событие о расчете дедлайна
                            await _eventBus.PublishAsync(
                                new ServiceDeadlineCalculatedEvent(
                                    @event.ServiceId,
                                    @event.ApplicationId,
                                    calculatedDeadline));
                        }
                    }
                }
                
                _logger.LogInformation("Successfully processed ApplicationCreatedEvent for application {ApplicationId}", @event.ApplicationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ApplicationCreatedEvent for application {ApplicationId}", @event.ApplicationId);
                throw; // Пусть RabbitMQ обработает ошибку и повторит доставку
            }
        }
    }

    /// <summary>
    /// Обработчик события изменения статуса заявки
    /// </summary>
    public class ApplicationStatusChangedEventHandler : IEventHandler<ApplicationStatusChangedEvent>
    {
        private readonly ILogger<ApplicationStatusChangedEventHandler> _logger;

        public ApplicationStatusChangedEventHandler(ILogger<ApplicationStatusChangedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(ApplicationStatusChangedEvent @event)
        {
            _logger.LogInformation(
                "Application {ApplicationId} status changed from {OldStatus} to {NewStatus}",
                @event.ApplicationId, @event.OldStatusId, @event.NewStatusId);

            // Дополнительная логика обработки изменения статуса
            // Например, отправка уведомлений пользователям
            
            await Task.CompletedTask;
        }
    }
}