using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data;
using System.Data.SqlClient;


/*
 * 
 * unused class, this project uses EF Core as the primary database handler. 
 * If we dont want to use EF Core, this should also work
 * https://hevodata.com/learn/c-sql-server/#Method_1_C_SQL_Server_Connection_using_Entity_Framework
*/

namespace PhoneBookApp.Database
{
    public interface IDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync();
    }

    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlServerConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
