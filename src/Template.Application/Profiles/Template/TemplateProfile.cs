using AutoMapper;

using Template.Application.Dto.Template;

using TemplateTestsDomain = Template.Domain.Entities.Template.Template;

namespace Template.Application.Profiles.Template;

public class TemplateProfile : Profile
{
    public TemplateProfile()
    {
        CreateMap<TemplateTestsDomain, TemplateDto>()
            .ReverseMap();
    }
}