namespace Template.Tests.Domain;

using TemplateEntity = Template.Domain.Entities.Template.Template;

public class TemplateTests
{
    private readonly TemplateEntity _template;

    public TemplateTests()
    {
        _template = new TemplateEntity
        {
            Active = true,
            Description = "mock",
            Id = 1,
            InsertionDate = DateTime.Now
        };
    }


    [Theory]
    [InlineData("descrição", false)]
    [InlineData("outra descrição", true)]
    public void Template_Update_Should_Update_Only_Description_Active(string description, bool active)
    {
        var templateMock = new TemplateEntity { Description = description, Active = active };
        _template.Update(templateMock);

        Assert.Equal(templateMock.Description, _template.Description);
        Assert.Equal(templateMock.Active, _template.Active);
        Assert.NotEqual(templateMock.Id, _template.Id);
        Assert.NotEqual(templateMock.InsertionDate, _template.InsertionDate);
    }

}
