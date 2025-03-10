using System.Data;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Infrastructure.Data.Models;
using Application.Exceptions;
using Application.Models;
using System;
using Infrastructure.FillLogData;
using System.Data.Common;


namespace Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {

        private IDbTransaction? _dbTransaction;
        private IDbConnection _dbConnection;

        public ServiceRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<List<Service>> GetAll()
        {
            try
            {
                var sql =
                    @"SELECT service.id, service.name, short_name, code, description, day_count, workflow_id, price, workflow.name as workflow_name, service.is_active
                                FROM service 
                                    left join workflow on workflow.id = service.workflow_id
                                ORDER BY service.name";
                var models = await _dbConnection.QueryAsync<Service>(sql, transaction: _dbTransaction);
                return models.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get Service", ex);
            }
        }

        public async Task<Service> GetOneByID(int id)
        {
            try
            {
                var sql =
                    "SELECT id, name, short_name, code, description, day_count, workflow_id, price FROM service WHERE id=@Id";
                var model = await _dbConnection.QuerySingleOrDefaultAsync<Service>(sql, new { Id = id },
                    transaction: _dbTransaction);

                if (model == null)
                {
                    throw new RepositoryException($"Service with ID {id} not found.", null);
                }

                return model;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get Service", ex);
            }
        }

        public async Task<int> Add(Service domain)
        {
            try
            {
                var model = new Service
                {
                    name = domain.name,
                    short_name = domain.short_name,
                    code = domain.code,
                    description = domain.description,
                    day_count = domain.day_count,
                    workflow_id = domain.workflow_id,
                    price = domain.price,
                };

                var sql =
                    "INSERT INTO service(name, short_name, code, description, day_count, workflow_id, price, created_at, updated_at, created_by, updated_by) VALUES " +
                    "(@name, @short_name, @code, @description, @day_count, @workflow_id, @price, @created_at, @updated_at, @created_by, @updated_by) RETURNING id";
                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return result;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to add Service", ex);
            }
        }

        public async Task Update(Service domain)
        {
            try
            {
                var model = new Service
                {
                    id = domain.id,
                    name = domain.name,
                    short_name = domain.short_name,
                    code = domain.code,
                    description = domain.description,
                    day_count = domain.day_count,
                    workflow_id = domain.workflow_id,
                    price = domain.price,
                };

                var sql = "UPDATE service SET name = @name, short_name = @short_name, code = @code," +
                          " description = @description, day_count = @day_count, workflow_id = @workflow_id, " +
                          "price = @price, updated_at = @updated_at, updated_by = @updated_by WHERE id = @id";
                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
                if (affected == 0)
                {
                    throw new RepositoryException("Not found", null);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to update Service", ex);
            }
        }

        public async Task<PaginatedList<Service>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = "SELECT * FROM service OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<Service>(sql, new { pageSize, pageNumber },
                    transaction: _dbTransaction);

                var sqlCount = "SELECT Count(*) FROM service";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var domainItems = models.ToList();

                return new PaginatedList<Service>(domainItems, totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get Service", ex);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var sql = "DELETE FROM service WHERE id = @Id";
                var affected = await _dbConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);

                if (affected == 0)
                {
                    throw new RepositoryException("Service not found", null);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to delete Service", ex);
            }
        }
    }
}