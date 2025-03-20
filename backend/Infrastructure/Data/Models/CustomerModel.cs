using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Models
{
    public class CustomerModel : BaseLogDomain
    {
        public int Id { get; set; }
        public string? Pin { get; set; }
        public bool? IsOrganization { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Director { get; set; }
        public string? Okpo { get; set; }
        public int? OrganizationTypeId { get; set; }
        public string? OrganizationTypeName { get; set; }
        public string? PaymentAccount { get; set; }
        public string? PostalCode { get; set; }
        public string? Ugns { get; set; }
        public string? Bank { get; set; }
        public string? Bik { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? IndividualName { get; set; }
        public string? IndividualSecondname { get; set; }
        public string? IndividualSurname { get; set; }

    }
}