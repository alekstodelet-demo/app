using System;
using Messaging.Shared.Events;

namespace Infrastructure.Messaging.Events
{
    /// <summary>
    /// Событие создания сервиса
    /// </summary>
    public class ServiceCreatedEvent : IntegrationEvent
    {
        public int ServiceId { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public int? DayCount { get; private set; }
        public decimal? Price { get; private set; }
        public int? WorkflowId { get; private set; }

        public ServiceCreatedEvent(
            int serviceId,
            string name,
            string code,
            string description,
            int? dayCount,
            decimal? price,
            int? workflowId) : base()
        {
            ServiceId = serviceId;
            Name = name;
            Code = code;
            Description = description;
            DayCount = dayCount;
            Price = price;
            WorkflowId = workflowId;
        }
    }

    /// <summary>
    /// Событие обновления сервиса
    /// </summary>
    public class ServiceUpdatedEvent : IntegrationEvent
    {
        public int ServiceId { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public int? DayCount { get; private set; }
        public decimal? Price { get; private set; }
        public int? WorkflowId { get; private set; }

        public ServiceUpdatedEvent(
            int serviceId,
            string name,
            string code,
            string description,
            int? dayCount,
            decimal? price,
            int? workflowId)
        {
            ServiceId = serviceId;
            Name = name;
            Code = code;
            Description = description;
            DayCount = dayCount;
            Price = price;
            WorkflowId = workflowId;
        }
    }

    /// <summary>
    /// Событие удаления сервиса
    /// </summary>
    public class ServiceDeletedEvent : IntegrationEvent
    {
        public int ServiceId { get; private set; }

        public ServiceDeletedEvent(int serviceId)
        {
            ServiceId = serviceId;
        }
    }

    /// <summary>
    /// Событие расчета дедлайна для сервиса
    /// </summary>
    public class ServiceDeadlineCalculatedEvent : IntegrationEvent
    {
        public int ServiceId { get; private set; }
        public int ApplicationId { get; private set; }
        public DateTime CalculatedDeadline { get; private set; }

        public ServiceDeadlineCalculatedEvent(
            int serviceId,
            int applicationId,
            DateTime calculatedDeadline)
        {
            ServiceId = serviceId;
            ApplicationId = applicationId;
            CalculatedDeadline = calculatedDeadline;
        }
    }
}