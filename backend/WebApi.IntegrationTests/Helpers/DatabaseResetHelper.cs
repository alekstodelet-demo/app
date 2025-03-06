using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebApi.IntegrationTests.Helpers
{
    public class DatabaseHelper
    {
        private static string _connectionString;

        static DatabaseHelper()
        {
            _connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

            if (string.IsNullOrEmpty(_connectionString))
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string has not been initialized.");
            }
        }

        public static void ClearTables()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(@"
                        DO $$ 
                        DECLARE 
                            r RECORD; 
                        BEGIN 
                            FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public') 
                            LOOP 
                                EXECUTE 'TRUNCATE TABLE public.""' || r.tablename || '"" RESTART IDENTITY CASCADE'; 
                            END LOOP; 
                        END 
                        $$;", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while clearing the tables.", ex);
            }
        }

        public static int RunQuery(string query)
        {
            object result;
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        result = command.ExecuteScalar();
                    }
                }
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while run query", ex);
            }
        }
        
        public static List<T> RunQueryList<T>(string query, Func<IDataReader, T> map)
        {
            var results = new List<T>();

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(map(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while running the query", ex);
            }

            return results;
        }
        
        public static int RunQuery(string query, Dictionary<string, object> parameters = null)
        {
            object result;
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // If there are parameters, add them to the command
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        result = command.ExecuteScalar();
                    }
                }
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while running the query", ex);
            }
        }

    }
}
