using Template.Infra.Repositories.SqlServer.Template;

namespace Template.Infra.Data.SqlServer;

public interface ISqlServerUnitOfWork : IDisposable
{
    void BeginTransaction();
    void CommitSession();
    Task CommitContextAsync();
    void Rollback();

    ITemplateRepository TemplateRepository { get; }
}
