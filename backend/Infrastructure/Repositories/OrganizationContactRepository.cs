using Application.Repositories;
using Domain.Entities;
using FluentResults;
using System.Data;

namespace Infrastructure.Repositories
{
    public class OrganizationContactRepository : BaseRepository, IOrganizationContactRepository
    {
        public OrganizationContactRepository(IDbConnection dbConnection, IDbTransaction dbTransaction) 
            : base(dbConnection, dbTransaction)
        {
        }

        public async Task<Result<IEnumerable<OrganizationContact>>> GetByOrganizationId(int organizationId)
        {
            const string sql = @"
                SELECT oc.id AS ""Id"",
                       oc.organization_id AS ""OrganizationId"",
                       oc.contact_type_id AS ""ContactTypeId"",
                       oc.value AS ""Value"",
                       oc.is_active AS ""IsActive"",
                       ct.name AS ""ContactTypeName""
                FROM organization_contacts oc
                JOIN contact_types ct ON ct.id = oc.contact_type_id
                WHERE oc.organization_id = @OrganizationId
                AND oc.is_active = true";

            return await QueryAsync<OrganizationContact>(sql, new { OrganizationId = organizationId });
        }

        public async Task<Result<int>> Add(OrganizationContact contact)
        {
            const string sql = @"
                INSERT INTO organization_contacts 
                (organization_id, contact_type_id, value, is_active, created_at, created_by)
                VALUES 
                (@OrganizationId, @ContactTypeId, @Value, @IsActive, @CreatedAt, @CreatedBy)
                RETURNING id";

            var param = new
            {
                contact.OrganizationId,
                contact.ContactTypeId,
                contact.Value,
                contact.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1
            };

            return await QuerySingleOrDefaultAsync<int>(sql, param);
        }

        public async Task<Result> Update(OrganizationContact contact)
        {
            const string sql = @"
                UPDATE organization_contacts 
                SET contact_type_id = @ContactTypeId,
                    value = @Value,
                    is_active = @IsActive,
                    updated_at = @UpdatedAt,
                    updated_by = @UpdatedBy
                WHERE id = @Id AND organization_id = @OrganizationId";

            var param = new
            {
                contact.Id,
                contact.OrganizationId,
                contact.ContactTypeId,
                contact.Value,
                contact.IsActive,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 1
            };

            var result = await ExecuteAsync(sql, param);
            return result.IsSuccess ? Result.Ok() : Result.Fail(result.Errors);
        }
    }
} 