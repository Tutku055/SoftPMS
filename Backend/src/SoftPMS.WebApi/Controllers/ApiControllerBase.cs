using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SoftPMS.WebApi.Controllers;

/// <summary>Base API controller with lazily resolved ISender.</summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender =>
        _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
