using Microsoft.AspNetCore.Mvc;

using Moq;
using Template.Api.Controllers.V2;
using Template.Application.Dto.Template;
using Template.Application.Services.Template;
using Template.Application.Shared.Notifications;

namespace Template.Tests.Controllers.V2;
public class TemplateControllerV2Tests
{
    private readonly Mock<NotificationContext> _mockNotification;
    private readonly Mock<ITemplateService> _mockTemplateService;
    private readonly TemplateController _templateController;

    public TemplateControllerV2Tests()
    {
        _mockNotification = new Mock<NotificationContext>();
        _mockTemplateService = new Mock<ITemplateService>();
        _templateController = new TemplateController(_mockNotification.Object, _mockTemplateService.Object);
    }

    private List<TemplateDto> TemplateDtoList()
    {
        return new List<TemplateDto>
        {
            new TemplateDto { Id = 1, Active = true},
            new TemplateDto { Id = 2, Active = false},
            new TemplateDto { Id = 1, Active = true}
        };
    }

    [Fact]
    public async void TemplateController_Should_Return_200()
    {
        //Arrange
        var dtoList = TemplateDtoList();
        _mockTemplateService.Setup(s => s.GetAllTemplates()).ReturnsAsync(dtoList);

        //Act
        var result = await _templateController.GetTemplateTests();

        Assert.NotNull(result);
        var objectResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsType<List<TemplateDto>>(objectResult.Value);
        Assert.Equal(dtoList, value);
    }

    [Fact]
    public async void TemplateController_Should_Thrown_Exception()
    {
        //Arrange
        var exception = new Exception();
        _mockTemplateService.Setup(s => s.GetAllTemplates()).ThrowsAsync(exception);

        //Act        
        var exceptionResult = await Assert.ThrowsAsync<Exception>(() => _templateController.GetTemplateTests());
        Assert.Equal(exception, exceptionResult);
    }
}
