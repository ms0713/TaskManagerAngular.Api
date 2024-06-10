using System.Data;

namespace TaskManagerAngular.Api.Data;
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}