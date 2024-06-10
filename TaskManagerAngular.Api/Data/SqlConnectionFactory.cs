using System.Data;
using System.Data.SqlClient;

namespace TaskManagerAngular.Api.Data;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string m_ConnectionString;

    public SqlConnectionFactory(string connectionString)
    {
        m_ConnectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(m_ConnectionString);
        connection.Open();

        return connection;
    }
}
