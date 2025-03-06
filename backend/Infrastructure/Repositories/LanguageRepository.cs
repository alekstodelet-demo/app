using System.Data;
using Dapper;
using Domain.Entities;
using Application.Repositories;
using Infrastructure.Data.Models;
using Application.Exceptions;
using Application.Models;
using System;
using Infrastructure.FillLogData;

namespace Infrastructure.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction? _dbTransaction;

        public LanguageRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public async Task<List<Language>> GetAll()
        {
            try
            {
                var sql = "SELECT * FROM \"Language\"";
                var models = await _dbConnection.QueryAsync<Language>(sql, transaction: _dbTransaction);
                return models.ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get Language", ex);
            }
        }

        public async Task<int> Add(Language domain)
        {
            try
            {
                var model = new LanguageModel
                {

                    name = domain.name,
                    description = domain.description,
                    code = domain.code,
                    isDefault = domain.isDefault,
                    queueNumber = domain.queueNumber,
                    id = domain.id,
                };

                

                var sql = "INSERT INTO \"Language\"(\"name\", \"description\", \"code\", \"isDefault\", \"queueNumber\", created_at, updated_at, created_by, updated_by) " +
                    "VALUES (@name, @description, @code, @isDefault, @queueNumber, @created_at, @updated_at, @created_by, @updated_by) RETURNING id";
                var result = await _dbConnection.ExecuteScalarAsync<int>(sql, model, transaction: _dbTransaction);
                return result;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to add Language", ex);
            }
        }

        public async Task Update(Language domain)
        {
            try
            {
                var model = new LanguageModel
                {

                    name = domain.name,
                    description = domain.description,
                    code = domain.code,
                    isDefault = domain.isDefault,
                    queueNumber = domain.queueNumber,
                    id = domain.id,
                };

                var sql = "UPDATE \"Language\" SET \"name\" = @name, \"description\" = @description, \"code\" = @code, \"isDefault\" = @isDefault, \"queueNumber\" = @queueNumber, \"id\" = @id, updated_at = @updated_at, updated_by = @updated_by WHERE id = @id";
                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
                if (affected == 0)
                {
                    throw new RepositoryException("Not found", null);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to update Language", ex);
            }
        }

        public async Task<PaginatedList<Language>> GetPaginated(int pageSize, int pageNumber)
        {
            try
            {
                var sql = "SELECT * FROM \"Language\" OFFSET @pageSize * (@pageNumber - 1) Limit @pageSize;";
                var models = await _dbConnection.QueryAsync<Language>(sql, new { pageSize, pageNumber }, transaction: _dbTransaction);

                var sqlCount = "SELECT Count(*) FROM \"Language\"";
                var totalItems = await _dbConnection.ExecuteScalarAsync<int>(sqlCount, transaction: _dbTransaction);

                var domainItems = models.ToList();

                return new PaginatedList<Language>(domainItems, totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get Languages", ex);
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                var model = new { id = id };
                var sql = "DELETE FROM \"Language\" WHERE id = @id";
                var affected = await _dbConnection.ExecuteAsync(sql, model, transaction: _dbTransaction);
                if (affected == 0)
                {
                    throw new RepositoryException("Not found", null);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to update Language", ex);
            }
        }
        public async Task<Language> GetOne(int id)
        {
            try
            {
                var sql = "SELECT * FROM \"Language\" WHERE id = @id LIMIT 1";
                var models = await _dbConnection.QueryAsync<Language>(sql, new { id }, transaction: _dbTransaction);
                return models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Failed to get Language", ex);
            }
        }


    }
}
