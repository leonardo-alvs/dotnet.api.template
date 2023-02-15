namespace Template.Infra.Data.Oracle;

public class OracleUnitOfWork : IOracleUnitOfWork
{
    private readonly OracleDbSession _session;

    public OracleUnitOfWork(OracleDbSession session)
    {
        _session = session;
    }

    public void BeginTransaction()
    {
        _session.Transaction = _session.Connection.BeginTransaction();
    }

    public void Commit()
    {
        _session.Transaction?.Commit();
    }

    public void Rollback()
    {
        _session.Transaction?.Rollback();
    }

    public void Dispose()
    {
        _session?.Dispose();
        _session?.Transaction?.Dispose();
        GC.SuppressFinalize(this);
    }
}
