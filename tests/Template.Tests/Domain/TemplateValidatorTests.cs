using Template.Domain.Entities.Template;

namespace Template.Tests.Domain;

using TemplateEntity = Template.Domain.Entities.Template.Template;

public class TemplateValidatorTests
{
    private readonly TemplateValidator _templateValidator;

    public TemplateValidatorTests()
    {
        _templateValidator = new TemplateValidator();
    }

    public static readonly object[][] CorrectObjects =
    {
        new object [] { 1, new DateTime(2000,01,01), "descrição", false },
        new object [] { 1, new DateTime(2100,01,01), "12345", false },
        new object [] { 1, new DateTime(2100,01,01), new string('A', 50), true }
    };

    public static readonly object[][] IncorrectObjects =
    {
        new object [] { 1, new DateTime(), "descrição", false },
        new object [] { 1, new DateTime(2100,01,01), "123", true },
        new object [] { 1, new DateTime(), "a", true }
    };


    [Theory]
    [MemberData(nameof(CorrectObjects))]
    public void TemplateValidator_Update_Should_Be_Valid(int id, DateTime insertionDate, string description, bool active)
    {
        var templateMock = new TemplateEntity
        {
            Id = id,
            Description = description,
            InsertionDate = Convert.ToDateTime(insertionDate),
            Active = active
        };

        var result = _templateValidator.Validate(templateMock);
        Assert.True(result.IsValid);
    }

    [Theory]
    [MemberData(nameof(IncorrectObjects))]
    public void TemplateValidator_Update_Should_Be_Invalid(int id, DateTime insertionDate, string description, bool active)
    {
        var templateMock = new TemplateEntity
        {
            Id = id,
            Description = description,
            InsertionDate = Convert.ToDateTime(insertionDate),
            Active = active
        };

        var result = _templateValidator.Validate(templateMock);
        Assert.False(result.IsValid);
    }

}
