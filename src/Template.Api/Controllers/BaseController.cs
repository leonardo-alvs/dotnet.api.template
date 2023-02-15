using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using Template.Application.Shared.Notifications;

namespace Template.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
    private readonly NotificationContext _notificationContext;

    protected BaseController(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }
}