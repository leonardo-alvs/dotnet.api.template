using Template.Infra.Repositories.SqlServer.Shared;
using TemplateTestsDomain = Template.Domain.Entities.Template.Template;

namespace Template.Infra.Repositories.SqlServer.Template;

public interface ITemplateRepository : IBaseRepository<TemplateTestsDomain>
{
    Task<List<TemplateTestsDomain>> GetActiveTemplates();
}