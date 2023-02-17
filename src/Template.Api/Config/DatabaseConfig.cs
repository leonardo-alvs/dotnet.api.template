using Microsoft.EntityFrameworkCore;
using Template.Infra.Data.Oracle;
using Template.Infra.Data.SqlServer;

namespace Template.Api.Config;

[ExcludeFromCodeCoverage]
public static class DatabaseConfig
{
    public static void AddDatabaseConfiguration(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddScoped<SqlServerDbSession>();
        services.AddScoped<OracleDbSession>();

        services.AddDbContext<SqlServerContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("ConnectionSql")));
    }
}