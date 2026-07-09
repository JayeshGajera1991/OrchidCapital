using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Orchid.UtilityHelper;
using System.Data;

namespace Orchid.DataAccess
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IConfiguration _config;
        private string DefaultConnectionstring = "DB-CONN-STR";

        SqlConnection connection = null;
        public int ConnectionString { get; set; }
        public string DB_NAME { get; set; }

        public DapperRepository(IConfiguration config)
        {
            _config = config;
        }
        public void Dispose()
        {

        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var conn = new SqlConnection(GetDbconnection(ConnectionString)))
            {
                return await conn.QueryAsync<T>(sp, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }
        public async Task<bool> ExecuteStoredProcedureAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var conn = new SqlConnection(GetDbconnection(ConnectionString)))
            {
                try
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync(sp, parms, commandType: commandType);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public async Task<T> ExecuteSPAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null)
        {
            using (var conn = new SqlConnection(GetDbconnection(ConnectionString)))
            {
                await conn.OpenAsync();
                return await conn.ExecuteScalarAsync<T>(sp, parms, commandType: commandType, commandTimeout: commandTimeout);
            }
        }

        public async Task<SqlMapper.GridReader> GetMultipleResults(string sp, DynamicParameters parms, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }
            connection = new SqlConnection(GetDbconnection(ConnectionString));

            await connection.OpenAsync();
            return await connection.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: commandTimeout);

        }

        public async Task<List<dynamic>> QueryMultipleResults(string sp, DynamicParameters parms, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure)
        {
            List<dynamic> dynamicResult = new List<dynamic>();
            var connection = new SqlConnection(GetDbconnection(ConnectionString));
            try
            {
                var _result = await connection.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: commandTimeout);
                while (!_result.IsConsumed)
                {
                    dynamicResult.Add(_result.Read<dynamic>());
                }
            }
            catch
            {

            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }
            return dynamicResult;
        }

        private string GetDbconnection(int connectionStringSwitcher)
        {
            string connection = string.Empty;
            connection = connectionStringSwitcher switch
            {
                1 => DefaultConnectionstring,
                _ => DefaultConnectionstring
            };

            var connectionString = _config[connection] ?? string.Empty;
            var decryptedConnectionString = string.Empty;
            try
            {
                // var key = Environment.GetEnvironmentVariable("USR_ENC_KEY");
                // if (!string.IsNullOrEmpty(key))
                // {
                //     decryptedConnectionString = Encryption.Decrypt(key, connectionString);
                // }
            }
            catch
            {
                decryptedConnectionString = string.Empty;
            }

            if (!string.IsNullOrEmpty(decryptedConnectionString))
            {
                connectionString = decryptedConnectionString;
            }

            var updatedConnectionString = connectionString.Replace("{{DBNAME}}", DB_NAME);
            if (!updatedConnectionString.Contains("Encrypt=", StringComparison.OrdinalIgnoreCase))
            {
                updatedConnectionString += ";Encrypt=False;";
            }
            return updatedConnectionString;

        }
    }
}
