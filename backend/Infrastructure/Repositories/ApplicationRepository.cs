using System.Data;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Application.Models;
using FluentResults;
using System;
using Infrastructure.FillLogData;

namespace Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;
        private IApplicationRepository _applicationRepositoryImplementation;

        public ApplicationRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<Result<List<Domain.Entities.Application>>> GetAll()
        {
            try
            {
                var sql = @"SELECT a.id AS ""Id"", 
                                   object_tag_id AS ""ObjectTagId"", 
                                   registration_date AS ""RegistrationDate"", 
                                   customer_id AS ""CustomerId"", 
                                   maria_db_statement_id AS ""MariaDbStatementId"",
                                   status_id AS ""StatusId"", 
                                   workflow_id AS ""WorkflowId"", 
                                   service_id AS ""ServiceId"", 
                                   deadline AS ""Deadline"", 
                                   arch_object_id AS ""ArchObjectId"", 
                                   work_description AS ""WorkDescription"", 
                                   ao.name AS ""ArchObjectName"",
                                   is_paid AS ""IsPaid"", 
                                   number AS ""Number""
                            FROM application a
                            LEFT JOIN arch_object ao ON ao.id = a.arch_object_id";
                var models = await _dbConnection.QueryAsync<Domain.Entities.Application>(sql, transaction: _dbTransaction);
                return Result.Ok(models.ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Applications", ex)
                    .WithMetadata("ErrorCode", "FETCH_ALL_FAILED"));
            }
        }
        
        public async Task<Result<Domain.Entities.Application>> GetOneByID(int id)
        {
            try
            {
                var sql = @"SELECT a.id AS ""Id"", 
                                   object_tag_id AS ""ObjectTagId"", 
                                   registration_date AS ""RegistrationDate"", 
                                   customer_id AS ""CustomerId"", 
                                   maria_db_statement_id AS ""MariaDbStatementId"",
                                   status_id AS ""StatusId"", 
                                   workflow_id AS ""WorkflowId"", 
                                   service_id AS ""ServiceId"", 
                                   deadline AS ""Deadline"", 
                                   arch_object_id AS ""ArchObjectId"", 
                                   work_description AS ""WorkDescription"", 
                                   arch_object.name AS ""ArchObjectName"",
                                   is_paid AS ""IsPaid"", 
                                   number AS ""Number""
                            FROM application a
                            LEFT JOIN arch_object ao ON ao.id = a.arch_object_id
                            WHERE a.id = @Id";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<Domain.Entities.Application>(sql, new { Id = id }, 
                    transaction: _dbTransaction);

                if (model == null)
                {
                    return Result.Fail(new ExceptionalError($"Application with ID {id} not found.", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }

                return Result.Ok(model);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Application", ex)
                    .WithMetadata("ErrorCode", "FETCH_ONE_FAILED"));
            }
        }

        public async Task<Result<int>> Add(Domain.Entities.Application domain)
        {
            try
            {
                var model = new
                {
                    RegistrationDate = DateTime.Now,
                    CustomerId = domain.CustomerId,
                    StatusId = domain.StatusId,
                    WorkflowId = domain.WorkflowId,
                    ServiceId = domain.ServiceId,
                    Deadline = domain.Deadline,
                    ArchObjectId = domain.ArchObjectId,
                    Number = domain.Number,
                    ObjectTagId = domain.ObjectTagId,
                    WorkDescription = domain.WorkDescription,
                    IncomingNumbers = domain.IncomingNumbers,
                    OutgoingNumbers = domain.OutgoingNumbers,
                    ApplicationCode = domain.ApplicationCode
                };

                var sql = @"INSERT INTO application (registration_date, customer_id, status_id, workflow_id, service_id,
                                                    deadline, created_at, arch_object_id, created_by, number, updated_at, 
                                                    work_description, object_tag_id, incoming_numbers, outgoing_numbers, application_code) 
                            VALUES (@RegistrationDate, @CustomerId, @StatusId, @WorkflowId, @ServiceId,
                                    @Deadline, @CreatedAt, @ArchObjectId, @CreatedBy, @Number, @UpdatedAt, 
                                    @WorkDescription, @ObjectTagId, @IncomingNumbers, @OutgoingNumbers, @ApplicationCode)
                            RETURNING id";
                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to add Application", ex)
                    .WithMetadata("ErrorCode", "ADD_FAILED"));
            }
        }

        public async Task<Result> Update(Domain.Entities.Application domain)
        {
            try
            {
                var model = new
                {
                    Id = domain.Id,
                    RegistrationDate = domain.RegistrationDate,
                    CustomerId = domain.CustomerId,
                    StatusId = domain.StatusId,
                    WorkflowId = domain.WorkflowId,
                    ServiceId = domain.ServiceId,
                    Deadline = domain.Deadline,
                    ArchObjectId = domain.ArchObjectId,
                    WorkDescription = domain.WorkDescription,
                    ObjectTagId = domain.ObjectTagId,
                    IncomingNumbers = domain.IncomingNumbers,
                    OutgoingNumbers = domain.OutgoingNumbers,
                    TechDecisionId = domain.TechDecisionId
                };

                var sql = @"UPDATE application 
                            SET registration_date = @RegistrationDate, 
                                customer_id = @CustomerId, 
                                status_id = @StatusId, 
                                workflow_id = @WorkflowId, 
                                service_id = @ServiceId, 
                                deadline = @Deadline, 
                                updated_at = @UpdatedAt, 
                                updated_by = @UpdatedBy,
                                arch_object_id = @ArchObjectId, 
                                work_description = @WorkDescription, 
                                object_tag_id = @ObjectTagId, 
                                incoming_numbers = @IncomingNumbers, 
                                outgoing_numbers = @OutgoingNumbers, 
                                tech_decision_id = @TechDecisionId
                            WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Application not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to update Application", ex)
                    .WithMetadata("ErrorCode", "UPDATE_FAILED"));
            }
        }

        public async Task<Result<PaginatedList<Domain.Entities.Application>>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = @"SELECT a.id AS ""Id"", 
                                   object_tag_id AS ""ObjectTagId"", 
                                   registration_date AS ""RegistrationDate"", 
                                   customer_id AS ""CustomerId"", 
                                   maria_db_statement_id AS ""MariaDbStatementId"",
                                   status_id AS ""StatusId"", 
                                   workflow_id AS ""WorkflowId"", 
                                   service_id AS ""ServiceId"", 
                                   deadline AS ""Deadline"", 
                                   arch_object_id AS ""ArchObjectId"", 
                                   work_description AS ""WorkDescription"", 
                                   arch_object.name AS ""ArchObjectName"",
                                   is_paid AS ""IsPaid"", 
                                   number AS ""Number""
                            FROM application a
                            LEFT JOIN arch_object ao ON ao.id = a.arch_object_id
                            OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<Domain.Entities.Application>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = @"SELECT Count(*) FROM application LEFT JOIN arch_object ao ON ao.id = a.arch_object_id";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var domainItems = models.ToList();

                return Result.Ok(new PaginatedList<Domain.Entities.Application>(domainItems, totalItems, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to get Service", ex)
                    .WithMetadata("ErrorCode", "FETCH_PAGINATED_FAILED"));
            }
        }

        public async Task<Result> Delete(int id)
        {
            try
            {
                var sql = "DELETE FROM application WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    return Result.Fail(new ExceptionalError("Application not found", null)
                        .WithMetadata("ErrorCode", "NOT_FOUND"));
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError("Failed to delete Application", ex)
                    .WithMetadata("ErrorCode", "DELETE_FAILED"));
            }
        }
    }
}