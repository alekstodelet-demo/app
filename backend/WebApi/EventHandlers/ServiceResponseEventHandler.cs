using FluentResults;
using Messaging.Shared;
using Messaging.Shared.Services.Events;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace WebApi.EventHandlers
{
    /// <summary>
    /// Обработчик событий ответа от микросервиса Services
    /// </summary>
    public class ServiceResponseEventHandler : IIntegrationEventHandler<ServiceResponseEvent>
    {
        private readonly ILogger<ServiceResponseEventHandler> _logger;
        private static readonly ConcurrentDictionary<Guid, TaskCompletionSource<ServiceResponseEvent>> _pendingRequests = 
            new ConcurrentDictionary<Guid, TaskCompletionSource<ServiceResponseEvent>>();

        public ServiceResponseEventHandler(ILogger<ServiceResponseEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Обрабатывает событие ответа от микросервиса Services
        /// </summary>
        public async Task<r> HandleAsync(ServiceResponseEvent @event)
        {
            _logger.LogInformation("Handling ServiceResponseEvent with correlationId: {@CorrelationId}", @event.CorrelationId);
            
            try
            {
                // Проверяем, есть ли ожидающий запрос с таким correlationId
                if (_pendingRequests.TryRemove(@event.CorrelationId, out var tcs))
                {
                    _logger.LogInformation("Found pending request for correlationId: {@CorrelationId}", @event.CorrelationId);
                    
                    // Устанавливаем результат для ожидающего запроса
                    tcs.SetResult(@event);
                }
                else
                {
                    _logger.LogWarning("No pending request found for correlationId: {@CorrelationId}", @event.CorrelationId);
                }
                
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ServiceResponseEvent");
                return Result.Fail(new Error("Error processing ServiceResponseEvent")
                    .WithMetadata("ErrorMessage", ex.Message));
            }
        }

        /// <summary>
        /// Регистрирует запрос и возвращает Task, который будет завершен при получении ответа
        /// </summary>
        public static Task<ServiceResponseEvent> RegisterRequest(Guid correlationId, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource<ServiceResponseEvent>();
            
            // Регистрируем запрос в словаре ожидающих запросов
            _pendingRequests[correlationId] = tcs;
            
            // Устанавливаем таймаут для запроса
            var cancellationTokenSource = new CancellationTokenSource(timeout);
            cancellationTokenSource.Token.Register(() => 
            {
                if (_pendingRequests.TryRemove(correlationId, out _))
                {
                    tcs.TrySetException(new TimeoutException($"Request with correlationId {correlationId} timed out after {timeout.TotalSeconds} seconds"));
                }
            });
            
            return tcs.Task;
        }
    }
}