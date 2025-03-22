
namespace Domain.Entities
{
    public class Application : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPin { get; set; }
        public int StatusId { get; set; }
        public int WorkflowId { get; set; }
        public int ServiceId { get; set; }
        public int? WorkflowTaskStructureId { get; set; }
        public string ServiceName { get; set; }
        public string? CreatedByName { get; set; }
        public string? AssignedEmployeesNames { get; set; }
        public string? UpdatedByName { get; set; }
        public DateTime? Deadline { get; set; }
        public int? ArchObjectId { get; set; }
        public string? ArchObjectName { get; set; }
        public string? ArchObjectAddress { get; set; }
        public string? ArchObjectDistrict { get; set; }
        public int? DistrictId { get; set; }
        public bool? IsPaid { get; set; }
        public string? Number { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public string? StatusColor { get; set; }
        public string ObjectTagName { get; set; }
        public bool? CustomerIsOrganization { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerOrganizationTypeName { get; set; }
        public string CustomerOkpo { get; set; }
        public string CustomerDirector { get; set; }
        public int? MariaDbStatementId { get; set; }
        public string? WorkDescription { get; set; }
        public string? CustomerContacts { get; set; }
        public List<ArchObject> ArchObjects { get; set; }
        public string? AssigneeEmployees { get; set; }
        public List<string> CustomersInfo { get; set; }
        public int RegistryId { get; set; }
        public int? ObjectTagId { get; set; }
        public string RegistryName { get; set; }
        public int? ArchProcessId { get; set; }
        public int? TechDecisionId { get; set; }
        public DateTime? TechDecisionDate { get; set; }
        public string IncomingNumbers { get; set; }
        public string OutgoingNumbers { get; set; }
        public decimal SumWithoutDiscount { get; set; }
        public decimal TotalSum { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal ServiceTaxValue { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal ServiceTaxPercentage { get; set; }
        public bool HasDiscount { get; set; }
        public int CalculationUpdatedBy { get; set; }
        public int CalculationCreatedBy { get; set; }        
        public DateTime? CalculationCreatedAt { get; set; }
        public DateTime? CalculationUpdatedAt { get; set; }
        public decimal TotalPaid { get; set; }
        public int? DayCount { get; set; }
        public string DecisionPaperOutgoingNumber { get; set; }
        public string? ApplicationCode { get; set; }
        public bool? MatchedContact { get; set; }
        public Validation Validate()
        {
            var errors = new List<FieldError>();
            if (ServiceId == 0)
            {
                errors.Add(new FieldError { ErrorCode = nameof(ErrorCode.COMMON_ID_NOT_NULL), FieldName = nameof(ServiceId) });
            }
            if (errors.Count > 0)
            {
                return Validation.NotValid(errors);
            }
            return Validation.Valid();
        }
    }
}
