using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Messaging.Shared;

namespace Infrastructure.Messaging
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IRabbitMQConnection _connection;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _exchangeName;
        
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private IModel _consumerChannel;

        public RabbitMQEventBus(
            IRabbitMQConnection connection,
            ILogger<RabbitMQEventBus> logger,
            IServiceProvider serviceProvider,
            string exchangeName = "bga_event_bus")
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _exchangeName = exchangeName;
            
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            
            // Создаем канал для получения сообщений
            _consumerChannel = CreateConsumerChannel();
        }

        public async Task PublishAsync<T>(T @event) where T : IEvent
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var eventName = @event.GetType().Name;
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            using var channel = _connection.CreateModel();
            
            // Объявляем exchange
            channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);

            // Добавляем свойства сообщения
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // Persistent
            properties.MessageId = @event.Id.ToString();
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            properties.ContentType = "application/json";

            _logger.LogInformation("Publishing event {EventName} to RabbitMQ: {EventJson}", eventName, message);

            // Публикуем сообщение
            channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body);

            await Task.CompletedTask;
        }

        public void Subscribe<T, TH>()
            where T : IEvent
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Contains(handlerType))
            {
                _logger.LogWarning("Handler Type {HandlerType} already registered for '{EventName}'", handlerType.Name, eventName);
                return;
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsume(eventName);

            _logger.LogInformation("Subscribed to event {EventName} with handler {HandlerType}", eventName, handlerType.Name);
        }

        public void Unsubscribe<T, TH>()
            where T : IEvent
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_handlers.ContainsKey(eventName))
            {
                return;
            }

            if (!_handlers[eventName].Contains(handlerType))
            {
                return;
            }

            _handlers[eventName].Remove(handlerType);

            if (_handlers[eventName].Count == 0)
            {
                _handlers.Remove(eventName);
                var queueName = GetQueueName(eventName);
                
                using var channel = _connection.CreateModel();
                channel.QueueUnbind(
                    queue: queueName,
                    exchange: _exchangeName,
                    routingKey: eventName);
                
                if (_handlers.Count == 0)
                {
                    _consumerChannel.Close();
                }
            }

            _logger.LogInformation("Unsubscribed from event {EventName} with handler {HandlerType}", eventName, handlerType.Name);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);

            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (_consumerChannel == null)
            {
                _consumerChannel = CreateConsumerChannel();
            }

            if (!_consumerChannel.IsOpen)
            {
                _consumerChannel = CreateConsumerChannel();
            }

            var queueName = GetQueueName(eventName);

            _consumerChannel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _consumerChannel.QueueBind(
                queue: queueName,
                exchange: _exchangeName,
                routingKey: eventName);

            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
            consumer.Received += ConsumerOnReceived;

            _consumerChannel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);
        }

        private async Task ConsumerOnReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                await ProcessEvent(eventName, message);
                
                // Подтверждаем получение сообщения
                _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error processing message: {Message}", message);
                
                // В случае ошибки возвращаем сообщение в очередь для повторной обработки
                _consumerChannel.BasicNack(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (!_handlers.ContainsKey(eventName))
            {
                _logger.LogWarning("No handlers registered for event {EventName}", eventName);
                return;
            }

            foreach (var handlerType in _handlers[eventName])
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetService(handlerType);
                
                if (handler == null)
                {
                    _logger.LogWarning("No handler instance available for {HandlerType}", handlerType.Name);
                    continue;
                }

                var eventType = _eventTypes.Find(t => t.Name == eventName);
                if (eventType == null)
                {
                    _logger.LogWarning("No event type registered for {EventName}", eventName);
                    continue;
                }

                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { integrationEvent });
                
                _logger.LogInformation("Handled event {EventName} with handler {HandlerType}", eventName, handlerType.Name);
            }
        }

        private string GetQueueName(string eventName)
        {
            return $"bga_{eventName}";
        }
    }
}