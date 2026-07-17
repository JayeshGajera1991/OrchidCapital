using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Orchid.DataAccess
{
    public interface IDapperRepository : IDisposable
    {
        Task InitializeDatabaseAsync();
        Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure);
        Task<bool> ExecuteStoredProcedureAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<SqlMapper.GridReader> GetMultipleResults(string sp, DynamicParameters parms, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure);
        Task<List<dynamic>> QueryMultipleResults(string sp, DynamicParameters parms, int? commandTimeout = null, CommandType commandType = CommandType.StoredProcedure);
        int ConnectionString { get; set; }
        string DB_NAME { get; set; }
        Task<T> ExecuteSPAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = null);
    }
}
