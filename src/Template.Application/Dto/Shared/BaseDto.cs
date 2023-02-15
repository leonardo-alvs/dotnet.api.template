using FluentValidation;
using System.Text.Json.Serialization;

using FluentValidation.Results;

namespace Template.Application.Dto.Shared;

public abstract class BaseDto
{
    public int Id { get; set; }
    [JsonIgnore]
    public bool Valid { get; private set; }
    [JsonIgnore]
    public bool Invalid => !Valid;
    [JsonIgnore]
    public ValidationResult ValidationResult { get; private set; }

    public bool Validate<TDto>(TDto dto, AbstractValidator<TDto> validator)
    {
        ValidationResult = validator.Validate(dto);
        return Valid = ValidationResult.IsValid;
    }
}
