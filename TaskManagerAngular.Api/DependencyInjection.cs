using Microsoft.EntityFrameworkCore;
using TaskManagerAngular.Api.Data;

namespace TaskManagerAngular.Api;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
                    configuration.GetConnectionString("Database") ??
                    throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));
    }
}
