using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SoftlarePMS.WebApi.Controllers;

/// <summary>
/// Abstract base controller that lazily resolves ISender from the DI container
/// via HttpContext so derived controllers need no constructor injection boilerplate.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    // Resolved on first access per request — avoids constructor coupling in every controller.
    private ISender? _sender;
    protected ISender Sender =>
        _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
