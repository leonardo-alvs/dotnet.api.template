using FluentValidation;

namespace Template.Domain.Entities.Template;

public class TemplateValidator : AbstractValidator<Template>
{
    public TemplateValidator()
    {
        RuleFor(t => t.Description)
            .NotEmpty()
            .Length(5, 50)
            .WithMessage(string.Format(DomainValidationMessages.LengthBetween,
                "Descrição", 5, 50));

        RuleFor(t => t.InsertionDate)
            .GreaterThan(DateTime.MinValue)
            .WithMessage(string.Format(DomainValidationMessages.InvalidFieldValue, "Data de cadastro"));
    }
}