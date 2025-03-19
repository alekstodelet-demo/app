using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Infrastructure.Messaging
{
    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }

    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMQConnection> _logger;
        private readonly int _retryCount;
        
        private IConnection _connection;
        private bool _disposed;
        private readonly object _sync = new();

        public RabbitMQConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQConnection> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = 5;
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public bool TryConnect()
        {
            _logger.LogInformation("Attempting to connect to RabbitMQ...");

            lock (_sync)
            {
                var retryAttempt = 0;
                while (!IsConnected && retryAttempt < _retryCount)
                {
                    retryAttempt++;
                    try
                    {
                        _connection = _connectionFactory.CreateConnection();
                        
                        // Обработка событий соединения
                        _connection.ConnectionShutdown += OnConnectionShutdown;
                        _connection.CallbackException += OnCallbackException;
                        _connection.ConnectionBlocked += OnConnectionBlocked;

                        _logger.LogInformation("Successfully connected to RabbitMQ on attempt {RetryAttempt}", retryAttempt);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "RabbitMQ connection attempt {RetryAttempt} of {RetryCount} failed", retryAttempt, _retryCount);
                        
                        // Ждем перед повторной попыткой
                        if (retryAttempt < _retryCount)
                        {
                            Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))).Wait();
                        }
                    }
                }

                if (!IsConnected)
                {
                    _logger.LogCritical("All connection attempts to RabbitMQ failed");
                }

                return IsConnected;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to create channel");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                if (_connection != null)
                {
                    _connection.ConnectionShutdown -= OnConnectionShutdown;
                    _connection.CallbackException -= OnCallbackException;
                    _connection.ConnectionBlocked -= OnConnectionBlocked;
                    _connection.Dispose();
                }
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex, "Error during disposing RabbitMQ connection");
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ connection is blocked. Reason: {0}", e.Reason);
            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ connection throw exception. Exception: {0}", e.Exception.Message);
            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("RabbitMQ connection is on shutdown. Reason: {0}", e.ReplyText);
            TryConnect();
        }
    }
}