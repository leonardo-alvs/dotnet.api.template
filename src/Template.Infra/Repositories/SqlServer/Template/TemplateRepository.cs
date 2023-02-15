using Dapper;

using TemplateTestsDomain = Template.Domain.Entities.Template.Template;
using Template.Infra.Data.SqlServer;
using Template.Infra.Repositories.SqlServer.Shared;

namespace Template.Infra.Repositories.SqlServer.Template;

public class TemplateRepository : BaseRepository<TemplateTestsDomain>, ITemplateRepository
{
    public TemplateRepository(SqlServerContext defaultContext, SqlServerDbSession session)
        : base(defaultContext, session)
    {

    }

    public async Task<List<TemplateTestsDomain>> GetActiveTemplates()
    {
        var templates = await Session.Connection.QueryAsync<TemplateTestsDomain>(TemplateStatements.GetActives, null, Session.Transaction);
        return templates.ToList();
    }

}