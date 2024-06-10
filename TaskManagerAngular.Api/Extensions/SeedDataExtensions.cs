using Bogus;
using Dapper;
using TaskManagerAngular.Api.Data;

namespace TaskManagerAngular.Api.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> projects = new();
        for (var i = 1; i <= 10; i++)
        {
            projects.Add(new
            {
                Id = i,
                Name = faker.Company.CompanyName(),
                DateOfStart = DateTime.UtcNow,
                TeamSize = faker.Random.Int(1,100),
            });
        }

        const string sql = """
            INSERT INTO dbo.projects
            (Id, "Name", DateOfStart, TeamSize)
            VALUES(@Id, @Name, @DateOfStart, @TeamSize);
            """;

        connection.Execute(sql, projects);
    }
}
