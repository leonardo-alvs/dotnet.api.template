using System.Net;

using Microsoft.AspNetCore.Mvc;

using Template.Application.Dto.Template;
using Template.Application.Services.Template;
using Template.Application.Shared.Notifications;

namespace Template.Api.Controllers.V1;

[ApiVersion("1.0", Deprecated = true)]
public class TemplateController : BaseController
{
    private readonly ITemplateService _templateTests;

    public TemplateController(NotificationContext notificationContext, ITemplateService templateTests)
        : base(notificationContext)
    {
        _templateTests = templateTests;
    }

    /// <summary>
    /// Get the templates
    /// </summary>
    /// <returns>Returns a collection of TemplateTestsDto</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<TemplateDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ICollection<TemplateDto>>> GetTemplateTests()
        => Ok(await _templateTests.GetAllTemplates());

    /// <summary>
    /// Get template by id
    /// </summary>
    /// <returns>Returns a TemplateTestsDto</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TemplateDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ICollection<TemplateDto>>> GetTemplateById(int id)
        => Ok(await _templateTests.GetTemplateById(id));

    /// <summary>
    /// Post template test
    /// </summary>
    /// <returns>Returns a SignatureDto</returns>
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CreateTemplate([FromBody] TemplateDto dto)
    {
        var result = await _templateTests.CreateTemplate(dto);
        return CreatedAtAction(nameof(CreateTemplate), result);
    }

    /// <summary>
    /// Put template test
    /// </summary>
    /// <returns>Returns a SignatureDto</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TemplateDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ICollection<Notification>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<TemplateDto>> CreateTemplate([FromBody] TemplateDto dto, int id)
        => Ok(await _templateTests.UpdateTemplate(id, dto));

    /// <summary>
    /// Delete a template test
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