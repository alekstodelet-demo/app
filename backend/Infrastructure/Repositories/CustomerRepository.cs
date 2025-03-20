using System.Data;
using System.Security.Cryptography;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Application.Models;
using FluentResults;
using Infrastructure.Data.Models;
using Infrastructure.Security;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;
        private EncryptionService _crypt;

        public CustomerRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _crypt = new EncryptionService(configuration);
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<Customer>>> GetAll()
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   pin AS ""Pin"",
                                   is_organization AS ""IsOrganization"",
                                   full_name AS ""FullName"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   address AS ""Address"",
                                   okpo AS ""Okpo"",
                                   director AS ""Director"",
                                   organization_type_id AS ""OrganizationTypeId"",
                                   payment_account AS ""PaymentAccount"",
                                   postal_code AS ""PostalCode"",
                                   ugns AS ""Ugns"",
                                   bank AS ""Bank"",
                                   bik AS ""Bik"",
                                   registration_number AS ""RegistrationNumber"",
                                   individual_name AS ""IndividualName"",
                                   individual_secondname AS ""IndividualSecondname"",
                                   individual_surname AS ""IndividualSurname"",
                                   identity_document_type_id AS ""IdentityDocumentTypeId"",
                                   document_serie AS ""DocumentSerie"",
                                   document_date_issue AS ""DocumentDateIssue"",
                                   document_whom_issued AS ""DocumentWhomIssued"",
                                   foreign_country AS ""ForeignCountry"",
                                   is_foreign AS ""IsForeign""
                            FROM Customer;";
                var models = await _dbConnection.QueryAsync<CustomerModel>(sql, transaction: _dbTransaction);

                var results = models.Select(model => DecryptCustomerModel(model)).ToList();

                return Result.Ok(results);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Customer", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }

        public async Task<Result<Customer>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                                   pin AS ""Pin"",
                                   is_organization AS ""IsOrganization"",
                                   full_name AS ""FullName"",
                                   created_at AS ""CreatedAt"",
                                   updated_at AS ""UpdatedAt"",
                                   created_by AS ""CreatedBy"",
                                   updated_by AS ""UpdatedBy"",
                                   address AS ""Address"",
                                   okpo AS ""Okpo"",
                                   director AS ""Director"",
                                   organization_type_id AS ""OrganizationTypeId"",
                                   payment_account AS ""PaymentAccount"",
                                   postal_code AS ""PostalCode"",
                                   ugns AS ""Ugns"",
                                   bank AS ""Bank"",
                                   bik AS ""Bik"",
                                   registration_number AS ""RegistrationNumber"",
                                   individual_name AS ""IndividualName"",
                                   individual_secondname AS ""IndividualSecondname"",
                                   individual_surname AS ""IndividualSurname"",
                                   identity_document_type_id AS ""IdentityDocumentTypeId"",
                                   document_serie AS ""DocumentSerie"",
                                   document_date_issue AS ""DocumentDateIssue"",
                                   document_whom_issued AS ""DocumentWhomIssued"",
                                   foreign_country AS ""ForeignCountry"",
                                   is_foreign AS ""IsForeign""
                            FROM Customer WHERE id=@Id;";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<CustomerModel>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Customer with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                var result = DecryptCustomerModel(model);

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Customer", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(Customer domain)
        {
            try
            {
                var model = EncryptCustomerModel(domain);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = 1;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;
                var sql = @"INSERT INTO Customer(pin, is_organization, full_name, created_at, updated_at, created_by, updated_by, address, okpo, 
                                 director, organization_type_id, payment_account, postal_code, ugns, bank, bik, registration_number, 
                                 individual_name, individual_secondname, individual_surname, identity_document_type_id, 
                                 document_serie, document_date_issue, document_whom_issued, foreign_country, is_foreign) 
            VALUES (@Pin, @IsOrganization, @FullName, @CreatedAt, @UpdatedAt, @CreatedBy, @UpdatedBy, @Address, @Okpo, 
                    @Director, @OrganizationTypeId, @PaymentAccount, @PostalCode, @Ugns, @Bank, @Bik, @RegistrationNumber, 
                    @IndividualName, @IndividualSecondname, @IndividualSurname, @IdentityDocumentTypeId, 
                    @DocumentSerie, @DocumentDateIssue, @DocumentWhomIssued, @ForeignCountry, @IsForeign) 
            RETURNING id";


                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add method error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Result.Fail(new ExceptionalError("Failed to add Customer", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(Customer domain)
        {
            try
            {
                var model = EncryptCustomerModel(domain);
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = 1;
                var sql = @"UPDATE Customer 
            SET pin = @Pin, 
                is_organization = @IsOrganization, 
                full_name = @FullName, 
                updated_at = @UpdatedAt, 
                updated_by = @UpdatedBy, 
                address = @Address, 
                okpo = @Okpo, 
                director = @Director, 
                organization_type_id = @OrganizationTypeId, 
                payment_account = @PaymentAccount, 
                postal_code = @PostalCode, 
                ugns = @Ugns, 
                bank = @Bank, 
                bik = @Bik, 
                registration_number = @RegistrationNumber, 
                individual_name = @IndividualName, 
                individual_secondname = @IndividualSecondname, 
                individual_surname = @IndividualSurname, 
                identity_document_type_id = @IdentityDocumentTypeId, 
                document_serie = @DocumentSerie, 
                document_date_issue = @DocumentDateIssue, 
                document_whom_issued = @DocumentWhomIssued, 
                foreign_country = @ForeignCountry, 
                is_foreign = @IsForeign
            WHERE id = @Id";

                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to update Customer", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<Customer>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT id AS ""Id"",
                   pin AS ""Pin"",
                   is_organization AS ""IsOrganization"",
                   full_name AS ""FullName"",
                   created_at AS ""CreatedAt"",
                   updated_at AS ""UpdatedAt"",
                   created_by AS ""CreatedBy"",
                   updated_by AS ""UpdatedBy"",
                   address AS ""Address"",
                   okpo AS ""Okpo"",
                   director AS ""Director"",
                   organization_type_id AS ""OrganizationTypeId"",
                   payment_account AS ""PaymentAccount"",
                   postal_code AS ""PostalCode"",
                   ugns AS ""Ugns"",
                   bank AS ""Bank"",
                   bik AS ""Bik"",
                   registration_number AS ""RegistrationNumber"",
                   individual_name AS ""IndividualName"",
                   individual_secondname AS ""IndividualSecondname"",
                   individual_surname AS ""IndividualSurname"",
                   identity_document_type_id AS ""IdentityDocumentTypeId"",
                   document_serie AS ""DocumentSerie"",
                   document_date_issue AS ""DocumentDateIssue"",
                   document_whom_issued AS ""DocumentWhomIssued"",
                   foreign_country AS ""ForeignCountry"",
                   is_foreign AS ""IsForeign""
            FROM Customer
            OFFSET @pageSize * (@pageNumber - 1) LIMIT @pageSize;";

                var models = await _dbConnection.QueryAsync<CustomerModel>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM Customer";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var results = models.Select(model => DecryptCustomerModel(model)).ToList();

                var domainItems = results;

                return Result.Ok(new PaginatedList<Customer>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Customer", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = @"DELETE FROM Customer WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Customer not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete Customer", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }

        private CustomerModel EncryptCustomerModel(Customer model)
        {
            return new CustomerModel
            {
                Id = model.Id,
                Pin = model.Pin, // Если нужно расшифровать: Decrypt(model.Pin)
                IsOrganization = model.IsOrganization,
                FullName = model.FullName,
                Address = model.Address,
                Director = model.Director,
                Okpo = model.Okpo,
                OrganizationTypeId = model.OrganizationTypeId,
                OrganizationTypeName = model.OrganizationTypeName,
                PaymentAccount = model.PaymentAccount,
                PostalCode = model.PostalCode,
                Ugns = model.Ugns,
                Bank = model.Bank,
                Bik = model.Bik,
                RegistrationNumber = model.RegistrationNumber,
                IndividualName = model.IndividualName,
                IndividualSecondname = model.IndividualSecondname,
                IndividualSurname = model.IndividualSurname,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
                IdentityDocumentTypeId = model.IdentityDocumentTypeId,
                DocumentDateIssue = model.DocumentDateIssue,
                DocumentSerie = model.DocumentSerie,
            };
        }
        private Customer DecryptCustomerModel(CustomerModel model)
        {
            return new Customer
            {
                Id = model.Id,
                Pin = model.Pin, // Если нужно расшифровать: Decrypt(model.Pin)
                IsOrganization = model.IsOrganization,
                FullName = model.FullName,
                Address = model.Address,
                Director = model.Director,
                Okpo = model.Okpo,
                OrganizationTypeId = model.OrganizationTypeId,
                OrganizationTypeName = model.OrganizationTypeName,
                PaymentAccount = model.PaymentAccount,
                PostalCode = model.PostalCode,
                Ugns = model.Ugns,
                Bank = model.Bank,
                Bik = model.Bik,
                RegistrationNumber = model.RegistrationNumber,
                IdentityDocumentTypeId = model.IdentityDocumentTypeId,
                DocumentDateIssue = model.DocumentDateIssue,
                DocumentSerie = model.DocumentSerie,
                IndividualName = model.IndividualName,
                IndividualSecondname = model.IndividualSecondname,
                IndividualSurname = model.IndividualSurname,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.UpdatedBy,
            };
        }

    }
}