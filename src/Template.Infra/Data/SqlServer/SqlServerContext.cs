using Microsoft.EntityFrameworkCore;

namespace Template.Infra.Data.SqlServer;

public class SqlServerContext : DbContext
{
    public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SqlServerContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}