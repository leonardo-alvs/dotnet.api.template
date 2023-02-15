using FluentValidation;

namespace Template.Application.Dto.Template;

public class TemplateDtoValidator : AbstractValidator<TemplateDto>
{
    public TemplateDtoValidator()
    {
        RuleFor(t => t.Description)
            .NotEmpty()
            .Length(5, 50)
            .WithMessage(string.Format(ApplicationValidationMessages.LengthBetween,
                "Descrição", 5, 50));

        RuleFor(t => t.InsertionDate)
            .GreaterThan(DateTime.MinValue)
            .WithMessage(string.Format(ApplicationValidationMessages.InvalidFieldValue, "Data de cadastro"));
    }
}