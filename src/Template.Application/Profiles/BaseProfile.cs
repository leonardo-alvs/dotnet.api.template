using AutoMapper;
using Template.Domain.Entities.Shared;
using Template.Application.Dto.Shared;

namespace Template.Application.Profiles;

public class BaseProfile : Profile
{
    public BaseProfile()
    {
        CreateMap<Entity, BaseDto>()
            .ReverseMap();
    }
}