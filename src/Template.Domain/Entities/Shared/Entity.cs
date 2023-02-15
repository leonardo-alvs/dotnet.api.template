using FluentValidation;
using FluentValidation.Results;

namespace Template.Domain.Entities.Shared;

public abstract class Entity
{
    public int Id { get; set; }
    public bool Valid { get; private set; }
    public bool Invalid => !Valid;
    public ValidationResult ValidationResult { get; private set; }

    public bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
    {
        ValidationResult = validator.Validate(model);
        return Valid = ValidationResult.IsValid;
    }
}