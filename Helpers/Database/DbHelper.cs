using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Helpers.Database
{
    public class DbHelper
    {
        private static string _connString = "";

        public static void Initialize(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection") ?? "";
            DefaultTypeMap.MatchNamesWithUnderscores = true;

        }

        public static async Task<int> ExecuteAsync(string sql, object model)
        {
            using (var connection = new NpgsqlConnection(_connString))
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(sql, model);
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql, object model)
        {
            using (var connection = new NpgsqlConnection(_connString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, model);
            }
        }

        public static async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object model)
        {
            using (var connection = new NpgsqlConnection(_connString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<T>(sql, model);
            }
        }
    }
}
