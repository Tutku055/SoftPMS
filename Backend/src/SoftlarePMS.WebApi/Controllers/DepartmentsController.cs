using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Department;
using SoftlarePMS.Application.Features.Departments.Commands.CreateDepartment;
using SoftlarePMS.Application.Features.Departments.Commands.DeleteDepartment;
using SoftlarePMS.Application.Features.Departments.Commands.UpdateDepartment;
using SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentById;
using SoftlarePMS.Application.Features.Departments.Queries.GetDepartmentsWithPagination;
using SoftlarePMS.WebApi.Authorization;

namespace SoftlarePMS.WebApi.Controllers;

[Authorize]
public sealed class DepartmentsController : ApiControllerBase
{
    /// <summary>Get a paginated list of departments.</summary>
    [HttpGet]
    [HasPermission("Departments.Read")]
    [ProducesResponseType(typeof(PaginatedList<DepartmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetDepartmentsWithPaginationQuery query, CancellationToken ct)
    {
        return Ok(await Sender.Send(query, ct));
    }

    /// <summary>Get a single department by id.</summary>
    [HttpGet("{id:guid}")]
    [HasPermission("Departments.Read")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await Sender.Send(new GetDepartmentByIdQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Create a new department.</summary>
    [HttpPost]
    [HasPermission("Departments.Create")]
    [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update a department.</summary>
    [HttpPut("{id:guid}")]
    [HasPermission("Departments.Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentCommand command, CancellationToken ct)
    {
        if (id != command.Id)
            return BadRequest(new { message = "Route id does not match command Id." });

        await Sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>Delete a department.</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission("Departments.Delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Sender.Send(new DeleteDepartmentCommand(id), ct);
        return NoContent();
    }
}
