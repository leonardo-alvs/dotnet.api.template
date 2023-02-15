namespace Template.Infra.Data.Oracle;

public interface IOracleUnitOfWork : IDisposable
{
    void BeginTransaction();
    void Commit();
    void Rollback();
}
