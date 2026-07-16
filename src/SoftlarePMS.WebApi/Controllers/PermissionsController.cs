using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftlarePMS.Application.DTOs.Permission;
using SoftlarePMS.Application.Features.Permissions.Queries.GetPermissions;
using SoftlarePMS.WebApi.Authorization;

namespace SoftlarePMS.WebApi.Controllers;

[Authorize]
public sealed class PermissionsController : ApiControllerBase
{
    /// <summary>Get all available system permissions.</summary>
    [HttpGet]
    [HasPermission("Permissions.Read")]
    [ProducesResponseType(typeof(IEnumerable<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        return Ok(await Sender.Send(new GetPermissionsQuery(), ct));
    }
}
