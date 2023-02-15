using System.Net;

using Microsoft.AspNetCore.Mvc;

using Template.Application.Dto.Template;
using Template.Application.Services.Template;
using Template.Application.Shared.Notifications;

namespace Template.Api.Controllers.V2;

[ApiVersion("2.0")]
public class TemplateController : BaseController
{
    private readonly ITemplateService _templateTests;

    public TemplateController(NotificationContext notificationContext, ITemplateService templateTests)
        : base(notificationContext)
    {
        _templateTests = templateTests;
    }

    /// <summary>
    /// Retorna todos os templates
    /// </summary>
    /// <returns>Retorna uma coleção TemplateTestsDto</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<TemplateDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ICollection<TemplateDto>>> GetTemplateTests()
        => Ok(await _templateTests.GetAllTemplates());

    /// <summary>
    /// Retorna todos os templates ativos
    /// </summary>
    /// <returns>Retorna uma coleção TemplateTestsDto</returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ICollection<TemplateDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ICollection<TemplateDto>>> GetActiveTemplateTests()
        => Ok(await _templateTests.GetActiveTemplates());

    /// <summary>
    /// Retorna um template pelo ID
    /// </summary>
    /// <returns>Retorna um TemplateTestsDto</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TemplateDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ICollection<TemplateDto>>> GetTemplateById(int id)
        => Ok(await _templateTests.GetTemplateById(id));

    /// <summary>
    /// Cria um template
    /// </summary>
    /// <returns>Retorna um TemplateTestsDto</returns>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CreateTemplate([FromBody] TemplateDto dto)
    {
        var result = await _templateTests.CreateTemplate(dto);
        return CreatedAtAction(nameof(CreateTemplate), result);
    }

    /// <summary>
    /// Atualiza um template
    /// </summary>
    /// <returns>Returns a TemplateTestsDto</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TemplateDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<TemplateDto>> CreateTemplate([FromBody] TemplateDto dto, int id)
        => Ok(await _templateTests.UpdateTemplate(id, dto));

    /// <summary>
    /// Deleta um template
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<TemplateDto>> DeleteTemplate(int id)
    {
        await _templateTests.DeleteTemplate(id);
        return Ok();
    }

}