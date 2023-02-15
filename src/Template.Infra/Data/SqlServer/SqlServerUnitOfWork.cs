using Template.Infra.Repositories.SqlServer.Template;

namespace Template.Infra.Data.SqlServer;

public class SqlServerUnitOfWork : ISqlServerUnitOfWork
{
    private readonly SqlServerDbSession _session;
    private readonly SqlServerContext _context;

    public SqlServerUnitOfWork(SqlServerDbSession session, SqlServerContext context)
    {
        _session = session;
        _context = context;
    }

    public void BeginTransaction()
    {
        _session.Transaction = _session.Connection.BeginTransaction();
    }

    public void CommitSession()
    {
        _session?.Transaction?.Commit();
    }

    public async Task CommitContextAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Rollback()
    {
        _session?.Transaction?.Rollback();
    }

    public void Dispose()
    {
        _session?.Dispose();
        _session?.Transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public ITemplateRepository TemplateRepository =>
        new TemplateRepository(_context, _session);
}
