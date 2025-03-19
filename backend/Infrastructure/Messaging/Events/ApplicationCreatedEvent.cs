using System;

namespace Infrastructure.Messaging.Events
{
    /// <summary>
    /// Событие создания заявки
    /// </summary>
    public class ApplicationCreatedEvent : IntegrationEvent
    {
        public int ApplicationId { get; private set; }
        public DateTime? RegistrationDate { get; private set; }
        public int CustomerId { get; private set; }
        public int StatusId { get; private set; }
        public int WorkflowId { get; private set; }
        public int ServiceId { get; private set; }
        public DateTime? Deadline { get; private set; }
        public int? ArchObjectId { get; private set; }
        public string Number { get; private set; }

        public ApplicationCreatedEvent(
            int applicationId,
            DateTime? registrationDate,
            int customerId,
            int statusId,
            int workflowId,
            int serviceId,
            DateTime? deadline,
            int? archObjectId,
            string number)
        {
            ApplicationId = applicationId;
            RegistrationDate = registrationDate;
            CustomerId = customerId;
            StatusId = statusId;
            WorkflowId = workflowId;
            ServiceId = serviceId;
            Deadline = deadline;
            ArchObjectId = archObjectId;
            Number = number;
        }
    }

    /// <summary>
    /// Событие обновления заявки
    /// </summary>
    public class ApplicationUpdatedEvent : IntegrationEvent
    {
        public int ApplicationId { get; private set; }
        public DateTime? RegistrationDate { get; private set; }
        public int CustomerId { get; private set; }
        public int StatusId { get; private set; }
        public int WorkflowId { get; private set; }
        public int ServiceId { get; private set; }
        public DateTime? Deadline { get; private set; }
        public int? ArchObjectId { get; private set; }
        public string Number { get; private set; }

        public ApplicationUpdatedEvent(
            int applicationId,
            DateTime? registrationDate,
            int customerId,
            int statusId,
            int workflowId,
            int serviceId,
            DateTime? deadline,
            int? archObjectId,
            string number)
        {
            ApplicationId = applicationId;
            RegistrationDate = registrationDate;
            CustomerId = customerId;
            StatusId = statusId;
            WorkflowId = workflowId;
            ServiceId = serviceId;
            Deadline = deadline;
            ArchObjectId = archObjectId;
            Number = number;
        }
    }

    /// <summary>
    /// Событие изменения статуса заявки
    /// </summary>
    public class ApplicationStatusChangedEvent : IntegrationEvent
    {
        public int ApplicationId { get; private set; }
        public int OldStatusId { get; private set; }
        public int NewStatusId { get; private set; }
        public string NewStatusName { get; private set; }
        public int UserId { get; private set; }
        public string Comment { get; private set; }

        public ApplicationStatusChangedEvent(
            int applicationId,
            int oldStatusId,
            int newStatusId,
            string newStatusName,
            int userId,
            string comment)
        {
            ApplicationId = applicationId;
            OldStatusId = oldStatusId;
            NewStatusId = newStatusId;
            NewStatusName = newStatusName;
            UserId = userId;
            Comment = comment;
        }
    }

    /// <summary>
    /// Событие добавления архитектурного объекта к заявке
    /// </summary>
    public class ApplicationObjectAddedEvent : IntegrationEvent
    {
        public int ApplicationId { get; private set; }
        public int ArchObjectId { get; private set; }
        public int ApplicationObjectId { get; private set; }

        public ApplicationObjectAddedEvent(
            int applicationId,
            int archObjectId,
            int applicationObjectId)
        {
            ApplicationId = applicationId;
            ArchObjectId = archObjectId;
            ApplicationObjectId = applicationObjectId;
        }
    }
}