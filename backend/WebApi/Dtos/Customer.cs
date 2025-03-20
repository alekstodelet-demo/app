using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetCustomerResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("pin")]
        public string Pin { get; set; }
        [JsonProperty("is_organization")]
        public bool? IsOrganization { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("okpo")]
        public string Okpo { get; set; }

        [JsonProperty("organization_type_id")]
        public int? OrganizationTypeId { get; set; }

        [JsonProperty("payment_account")]
        public string PaymentAccount { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("ugns")]
        public string Ugns { get; set; }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("bik")]
        public string Bik { get; set; }

        [JsonProperty("registration_number")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("document_date_issue")]
        public DateTime? DocumentDateIssue { get; set; }

        [JsonProperty("document_serie")]
        public string DocumentSerie { get; set; }

        [JsonProperty("identity_document_type_id")]
        public int IdentityDocumentTypeId { get; set; }

        [JsonProperty("document_whom_issued")]
        public string DocumentWhomIssued { get; set; }

        [JsonProperty("individual_surname")]
        public string IndividualSurname { get; set; }

        [JsonProperty("individual_name")]
        public string IndividualName { get; set; }

        [JsonProperty("individual_secondname")]
        public string IndividualSecondname { get; set; }

        [JsonProperty("is_foreign")]
        public bool IsForeign { get; set; }

        [JsonProperty("foreign_country")]
        public string ForeignCountry { get; set; }


        internal static GetCustomerResponse FromDomain(Customer domain)
        {
            return new GetCustomerResponse
            {
                Id = domain.Id,
                Pin = domain.Pin,
                FullName = domain.FullName,
                IsOrganization = domain.IsOrganization,
                Address = domain.Address,
                Director = domain.Director,
                Okpo = domain.Okpo,
                OrganizationTypeId = domain.OrganizationTypeId,
                PaymentAccount = domain.PaymentAccount,
                PostalCode = domain.PostalCode,
                Ugns = domain.Ugns,
                Bank = domain.Bank,
                Bik = domain.Bik,
                RegistrationNumber = domain.RegistrationNumber,
                IndividualName = domain.IndividualName,
                IndividualSecondname = domain.IndividualSecondname,
                IndividualSurname = domain.IndividualSurname,

            };
        }
    }

    public class CreateCustomerRequest
    {

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("is_organization")]
        public bool? IsOrganization { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("okpo")]
        public string Okpo { get; set; }

        [JsonProperty("organization_type_id")]
        public int? OrganizationTypeId { get; set; }

        [JsonProperty("payment_account")]
        public string PaymentAccount { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("ugns")]
        public string Ugns { get; set; }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("bik")]
        public string Bik { get; set; }

        [JsonProperty("registration_number")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("document_date_issue")]
        public DateTime? DocumentDateIssue { get; set; }

        [JsonProperty("document_serie")]
        public string DocumentSerie { get; set; }

        [JsonProperty("identity_document_type_id")]
        public int IdentityDocumentTypeId { get; set; }

        [JsonProperty("document_whom_issued")]
        public string DocumentWhomIssued { get; set; }

        [JsonProperty("individual_surname")]
        public string IndividualSurname { get; set; }

        [JsonProperty("individual_name")]
        public string IndividualName { get; set; }

        [JsonProperty("individual_secondname")]
        public string IndividualSecondname { get; set; }

        [JsonProperty("is_foreign")]
        public bool IsForeign { get; set; }

        [JsonProperty("foreign_country")]
        public string ForeignCountry { get; set; }

        internal Customer ToDomain()
        {
            return new Customer
            {
                Pin = Pin,
                IsOrganization = IsOrganization,
                Address = Address,
                Director = Director,
                Okpo = Okpo,
                OrganizationTypeId = OrganizationTypeId,
                PaymentAccount = PaymentAccount,
                PostalCode = PostalCode,
                Ugns = Ugns,
                Bank = Bank,
                Bik = Bik,
                RegistrationNumber = RegistrationNumber,
                IndividualName = IndividualName,
                IndividualSecondname = IndividualSecondname,
                IndividualSurname = IndividualSurname,
            };
        }
    }

    public class UpdateCustomerRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("is_organization")]
        public bool? IsOrganization { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("okpo")]
        public string Okpo { get; set; }

        [JsonProperty("organization_type_id")]
        public int? OrganizationTypeId { get; set; }

        [JsonProperty("payment_account")]
        public string PaymentAccount { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("ugns")]
        public string Ugns { get; set; }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("bik")]
        public string Bik { get; set; }

        [JsonProperty("registration_number")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("document_date_issue")]
        public DateTime? DocumentDateIssue { get; set; }

        [JsonProperty("document_serie")]
        public string DocumentSerie { get; set; }

        [JsonProperty("identity_document_type_id")]
        public int IdentityDocumentTypeId { get; set; }

        [JsonProperty("document_whom_issued")]
        public string DocumentWhomIssued { get; set; }

        [JsonProperty("individual_surname")]
        public string IndividualSurname { get; set; }

        [JsonProperty("individual_name")]
        public string IndividualName { get; set; }

        [JsonProperty("individual_secondname")]
        public string IndividualSecondname { get; set; }

        [JsonProperty("is_foreign")]
        public bool IsForeign { get; set; }

        [JsonProperty("foreign_country")]
        public string ForeignCountry { get; set; }

        internal Customer ToDomain()
        {
            return new Customer
            {
                Id = Id,
                Pin = Pin,
                IsOrganization = IsOrganization,
                Address = Address,
                Director = Director,
                Okpo = Okpo,
                OrganizationTypeId = OrganizationTypeId,
                PaymentAccount = PaymentAccount,
                PostalCode = PostalCode,
                Ugns = Ugns,
                Bank = Bank,
                Bik = Bik,
                RegistrationNumber = RegistrationNumber,
                IndividualName = IndividualName,
                IndividualSecondname = IndividualSecondname,
                IndividualSurname = IndividualSurname,
            };
        }
    }
}
