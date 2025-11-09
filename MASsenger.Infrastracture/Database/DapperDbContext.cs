using Microsoft.Data.Sqlite;
using System.Data;

namespace MASsenger.Infrastracture.Database
{
    public class DapperDbContext
    {
        private readonly string _connectionString;
        private IDbConnection? _connection;
        public DapperDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqliteConnection(_connectionString);
            }
            return _connection;
        }
    }
}
