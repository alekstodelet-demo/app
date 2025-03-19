using System;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    /// <summary>
    /// Базовый интерфейс для всех событий
    /// </summary>
    public interface IEvent
    {
        Guid Id { get; }
        string EventType { get; }
        DateTime Timestamp { get; }
    }

    /// <summary>
    /// Абстрактный класс для реализации базового события
    /// </summary>
    public abstract class Event : IEvent
    {
        public Guid Id { get; private set; }
        public string EventType => GetType().Name;
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Интерфейс для интеграционных событий
    /// </summary>
    public interface IIntegrationEvent : IEvent
    {
        // Общие свойства для интеграционных событий
        string Source { get; }
        int Version { get; }
    }

    /// <summary>
    /// Базовый класс для интеграционных событий между микросервисами
    /// </summary>
    public abstract class IntegrationEvent : Event, IIntegrationEvent
    {
        public string Source { get; private set; }
        public int Version { get; private set; }

        protected IntegrationEvent(string source = "BGA", int version = 1)
        {
            Source = source;
            Version = version;
        }
    }

    /// <summary>
    /// Интерфейс обработчика событий
    /// </summary>
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }

    /// <summary>
    /// Интерфейс шины событий
    /// </summary>
    public interface IEventBus
    {
        // Публикация события
        Task PublishAsync<T>(T @event) where T : IEvent;
        
        // Подписка на событие
        void Subscribe<T, TH>()
            where T : IEvent
            where TH : IEventHandler<T>;
        
        // Отписка от события
        void Unsubscribe<T, TH>()
            where T : IEvent
            where TH : IEventHandler<T>;
    }
}