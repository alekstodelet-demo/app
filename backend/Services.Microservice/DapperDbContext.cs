using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Npgsql;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}