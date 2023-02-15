using AutoMapper;
using Moq;
using Template.Application.Dto.Template;
using Template.Application.Services.Template;
using Template.Application.Shared.Notifications;
using Template.Infra.Data.SqlServer;
using TemplateEntity = Template.Domain.Entities.Template.Template;

namespace Template.Tests.Services;
public class TemplateServiceTests
{
    private readonly Mock<NotificationContext> _mockNotification;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ISqlServerUnitOfWork> _mockUnitOfWork;
    private readonly TemplateService _templateService;

    public TemplateServiceTests()
    {
        _mockNotification = new Mock<NotificationContext>();
        _mockMapper = new Mock<IMapper>();
        _mockUnitOfWork = new Mock<ISqlServerUnitOfWork>();
        _templateService = new TemplateService(_mockNotification.Object, _mockMapper.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async void TemplateService_GetTemplateById_Should_Return_Template()
    {
        //Arrange
        var id = 1;
        var templateMock = new TemplateEntity();
        var templateDtoMock = new TemplateDto();

        _mockUnitOfWork.Setup(s => s.TemplateRepository.GetById(id)).ReturnsAsync(templateMock);
        _mockMapper.Setup(s => s.Map<TemplateDto>(It.IsAny<TemplateEntity>())).Returns(templateDtoMock);

        //Act
        var result = await _templateService.GetTemplateById(id);

        //Assert
        Assert.IsAssignableFrom<TemplateDto>(result);
    }

    [Fact]
    public async void TemplateService_GetTemplateById_Should_Return_Null()
    {
        //Arrange
        var id = 1;
        var templateMock = new TemplateEntity();
        var templateDtoMock = new TemplateDto();

        _mockUnitOfWork.Setup(s => s.TemplateRepository.GetById(id)).ReturnsAsync((TemplateEntity)null);

        //Act
        var result = await _templateService.GetTemplateById(id);

        //Assert
        Assert.Null(result);
    }

}
