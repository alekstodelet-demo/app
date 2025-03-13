using Application.Repositories;
using Dapper;
using Domain;
using Domain.Entities;
using Infrastructure.Repositories;
using System.Data;
using System.Data.Common;

namespace Infrastructure.FillLogData
{
    public static class FillLogDataHelper
    {
           public static  async Task FillLogDataCreate(BaseLogDomain domain, int userId)
        {
            domain.CreatedBy = userId;
            domain.CreatedAt = DateTime.Now;
            domain.UpdatedBy = userId;
            domain.UpdatedAt = DateTime.Now;
        }

        public static async Task FillLogDataUpdate(BaseLogDomain domain, int userId)
        {

            domain.UpdatedBy = userId;
            domain.UpdatedAt = DateTime.Now;
        }
    }
}
