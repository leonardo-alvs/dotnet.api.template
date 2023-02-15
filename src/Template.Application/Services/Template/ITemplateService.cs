using Template.Application.Dto.Template;
using Template.Application.Services.Shared;

namespace Template.Application.Services.Template;

public interface ITemplateService : IBaseService
{
    Task<TemplateDto> GetTemplateById(int id);
    Task<TemplateDto?> CreateTemplate(TemplateDto dto);
    Task DeleteTemplate(int id);
    Task<TemplateDto?> UpdateTemplate(int id, TemplateDto dto);
    Task<ICollection<TemplateDto>> GetAllTemplates();
    Task<ICollection<TemplateDto>> GetActiveTemplates();
}