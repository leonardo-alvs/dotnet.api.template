using FluentValidation;
using Template.Domain.Entities.Shared;
using Template.Application.Dto.Shared;
using Template.Application.Shared.Notifications;

namespace Template.Application.Services.Shared;

public abstract class BaseService : IBaseService
{
    internal readonly NotificationContext NotificationContext;

    protected BaseService(NotificationContext notificationContext)
    {
        NotificationContext = notificationContext;
    }

    public void ValidateDto<TDto>(TDto dto, AbstractValidator<TDto> validator) where TDto : BaseDto
    {
        dto.Validate(dto, validator);

        if (dto.Invalid)
        {
            NotificationContext.AddNotifications(dto.ValidationResult);
        }
    }

    public void ValidateDomainEntity<TModel>(TModel model, AbstractValidator<TModel> validator) where TModel : Entity
    {
        model.Validate(model, validator);

        if (model.Invalid)
        {
            NotificationContext.AddNotifications(model.ValidationResult);
        }
    }
}