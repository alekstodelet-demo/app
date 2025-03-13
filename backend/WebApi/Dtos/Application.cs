using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetApplicationResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("registration_date")]
        public DateTime? RegistrationDate { get; set; }
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }
        [JsonProperty("customer_name")]
        public string? CustomerName { get; set; }
        [JsonProperty("customer_pin")]
        public string? CustomerPin { get; set; }
        [JsonProperty("status_id")]
        public int StatusId { get; set; }
        [JsonProperty("workflow_id")]
        public int WorkflowId { get; set; }
        [JsonProperty("service_id")]
        public int ServiceId { get; set; }
        [JsonProperty("workflow_task_structure_id")]
        public int? WorkflowTaskStructureId { get; set; }
        [JsonProperty("service_name")]
        public string? ServiceName { get; set; }
        [JsonProperty("created_by_name")]
        public string? CreatedByName { get; set; }
        [JsonProperty("assigned_employees_names")]
        public string? AssignedEmployeesNames { get; set; }
        [JsonProperty("updated_by_name")]
        public string? UpdatedByName { get; set; }
        [JsonProperty("deadline")]
        public DateTime? Deadline { get; set; }
        [JsonProperty("arch_object_id")]
        public int? ArchObjectId { get; set; }
        [JsonProperty("arch_object_name")]
        public string? ArchObjectName { get; set; }
        [JsonProperty("arch_object_address")]
        public string? ArchObjectAddress { get; set; }
        [JsonProperty("arch_object_district")]
        public string? ArchObjectDistrict { get; set; }
        [JsonProperty("district_id")]
        public int? DistrictId { get; set; }
        [JsonProperty("is_paid")]
        public bool? IsPaid { get; set; }
        [JsonProperty("number")]
        public string? Number { get; set; }
        [JsonProperty("status_name")]
        public string? StatusName { get; set; }
        [JsonProperty("status_code")]
        public string? StatusCode { get; set; }
        [JsonProperty("status_color")]
        public string? StatusColor { get; set; }
        [JsonProperty("object_tag_name")]
        public string? ObjectTagName { get; set; }
        [JsonProperty("customer_is_organization")]
        public bool? CustomerIsOrganization { get; set; }
        [JsonProperty("customer_address")]
        public string? CustomerAddress { get; set; }
        [JsonProperty("customer_organization_type_name")]
        public string? CustomerOrganizationTypeName { get; set; }
        [JsonProperty("customer_okpo")]
        public string? CustomerOkpo { get; set; }
        [JsonProperty("customer_director")]
        public string? CustomerDirector { get; set; }
        [JsonProperty("total_sum")]
        public decimal TotalSum { get; set; }

        internal static GetApplicationResponse FromDomain(Domain.Entities.Application domain)
        {
            return new GetApplicationResponse
            {
                Id = domain.Id,
                RegistrationDate = domain.RegistrationDate,
                CustomerId = domain.CustomerId,
                CustomerName = domain.CustomerName,
                CustomerPin = domain.CustomerPin,
                StatusId = domain.StatusId,
                WorkflowId = domain.WorkflowId,
                ServiceId = domain.ServiceId,
                WorkflowTaskStructureId = domain.WorkflowTaskStructureId,
                ServiceName = domain.ServiceName,
                CreatedByName = domain.CreatedByName,
                AssignedEmployeesNames = domain.AssignedEmployeesNames,
                UpdatedByName = domain.UpdatedByName,
                Deadline = domain.Deadline,
                ArchObjectId = domain.ArchObjectId,
                ArchObjectName = domain.ArchObjectName,
                ArchObjectAddress = domain.ArchObjectAddress,
                ArchObjectDistrict = domain.ArchObjectDistrict,
                DistrictId = domain.DistrictId,
                IsPaid = domain.IsPaid,
                Number = domain.Number,
                StatusName = domain.StatusName,
                StatusCode = domain.StatusCode,
                StatusColor = domain.StatusColor,
                ObjectTagName = domain.ObjectTagName,
                CustomerIsOrganization = domain.CustomerIsOrganization,
                CustomerAddress = domain.CustomerAddress,
                CustomerOrganizationTypeName = domain.CustomerOrganizationTypeName,
                CustomerOkpo = domain.CustomerOkpo,
                CustomerDirector = domain.CustomerDirector,
                TotalSum = domain.TotalSum
            };
        }
    }

    public class CreateApplicationRequest
    {
        public DateTime? RegistrationDate { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPin { get; set; }
        public int StatusId { get; set; }
        public int WorkflowId { get; set; }
        public int ServiceId { get; set; }
        public int? WorkflowTaskStructureId { get; set; }
        public string? ServiceName { get; set; }
        public DateTime? Deadline { get; set; }
        public int? ArchObjectId { get; set; }
        public bool? IsPaid { get; set; }
        public string? Number { get; set; }
        public bool? CustomerIsOrganization { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerOkpo { get; set; }
        public string? CustomerDirector { get; set; }
        public decimal TotalSum { get; set; }

        internal Domain.Entities.Application ToDomain()
        {
            return new Domain.Entities.Application
            {
                RegistrationDate = RegistrationDate,
                CustomerId = CustomerId,
                CustomerName = CustomerName,
                CustomerPin = CustomerPin,
                StatusId = StatusId,
                WorkflowId = WorkflowId,
                ServiceId = ServiceId,
                WorkflowTaskStructureId = WorkflowTaskStructureId,
                ServiceName = ServiceName,
                Deadline = Deadline,
                ArchObjectId = ArchObjectId,
                IsPaid = IsPaid,
                Number = Number,
                CustomerIsOrganization = CustomerIsOrganization,
                CustomerAddress = CustomerAddress,
                CustomerOkpo = CustomerOkpo,
                CustomerDirector = CustomerDirector,
                TotalSum = TotalSum
            };
        }
    }

    public class UpdateApplicationRequest
    {
        public int Id { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPin { get; set; }
        public int StatusId { get; set; }
        public int WorkflowId { get; set; }
        public int ServiceId { get; set; }
        public int? WorkflowTaskStructureId { get; set; }
        public string? ServiceName { get; set; }
        public DateTime? Deadline { get; set; }
        public int? ArchObjectId { get; set; }
        public bool? IsPaid { get; set; }
        public string? Number { get; set; }
        public bool? CustomerIsOrganization { get; set; }
        public string? CustomerAddress { get; set; }
        public string? CustomerOkpo { get; set; }
        public string? CustomerDirector { get; set; }
        public decimal TotalSum { get; set; }

        internal Domain.Entities.Application ToDomain()
        {
            return new Domain.Entities.Application
            {
                Id = Id,
                RegistrationDate = RegistrationDate,
                CustomerId = CustomerId,
                CustomerName = CustomerName,
                CustomerPin = CustomerPin,
                StatusId = StatusId,
                WorkflowId = WorkflowId,
                ServiceId = ServiceId,
                WorkflowTaskStructureId = WorkflowTaskStructureId,
                ServiceName = ServiceName,
                Deadline = Deadline,
                ArchObjectId = ArchObjectId,
                IsPaid = IsPaid,
                Number = Number,
                CustomerIsOrganization = CustomerIsOrganization,
                CustomerAddress = CustomerAddress,
                CustomerOkpo = CustomerOkpo,
                CustomerDirector = CustomerDirector,
                TotalSum = TotalSum
            };
        }
    }
}