using Template.Application.Dto.Shared;

namespace Template.Application.Dto.Template;

public class TemplateDto : BaseDto
{
    public string Description { get; set; }
    public bool Active { get; set; }
    public DateTime InsertionDate { get; set; }

    public TemplateDto()
    {
        Validate(this, new TemplateDtoValidator());
    }
}