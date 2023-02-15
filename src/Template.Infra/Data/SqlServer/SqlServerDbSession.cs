using System.Data;
using System.Data.SqlClient;

using Microsoft.Extensions.Configuration;

namespace Template.Infra.Data.SqlServer;

public sealed class SqlServerDbSession : IDisposable
{
    private readonly IConfiguration _configuration;
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; set; }

    public SqlServerDbSession(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetValue<string>("ConnectionStrings:ConnectionSql");
        Connection = new SqlConnection(connectionString);
        Connection.Open();
    }

    public void Dispose() => Connection?.Dispose();
}