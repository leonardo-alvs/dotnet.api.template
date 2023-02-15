using Microsoft.Extensions.Configuration;

using Oracle.ManagedDataAccess.Client;

using System.Data;

namespace Template.Infra.Data.Oracle;

public sealed class OracleDbSession : IDisposable
{
    private readonly IConfiguration _configuration;
    public IDbConnection Connection { get; }
    public IDbTransaction? Transaction { get; set; }

    public OracleDbSession(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetValue<string>("ConnectionStrings:ConnectionOracle");
        Connection = new OracleConnection(connectionString);
        Connection.Open();
    }

    public void Dispose() => Connection?.Dispose();
}
