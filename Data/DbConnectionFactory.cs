using FirebirdSql.Data.FirebirdClient;
using System.Data;

namespace ProjetoCompleto.Data
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public IDbConnection CreateConnection()
        {
            return new FbConnection(_connectionString);
        }
    }
}
