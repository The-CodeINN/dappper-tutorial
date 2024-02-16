using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Dapper.Data
{
    public class DataContextDapper
    {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                dbConnection.Open();
                using (IDbTransaction transaction = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var holdValue = dbConnection.Query<T>(sql, null, transaction: transaction, commandTimeout: 999999999);
                    return holdValue;
                }
            }
        }

        public int ExecuteSQL(string sql)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return dbConnection.Execute(sql);
            }
        }

        public void ExecuteProcedureMulti(string sql, IDbConnection dbConnection)
        {
            dbConnection.Execute(sql);
        }
    }
}