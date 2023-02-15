using FluentValidation;
using Template.Domain.Entities.Shared;
using Template.Application.Dto.Shared;

namespace Template.Application.Services.Shared;

public interface IBaseService
{
    void ValidateDto<TDto>(TDto dto, AbstractValidator<TDto> validator) where TDto : BaseDto;
    void ValidateDomainEntity<TModel>(TModel model, AbstractValidator<TModel> validator) where TModel : Entity;
}
