using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Application.DTOs.Employee;
using SoftlarePMS.Application.Features.Employees.Commands.CreateEmployee;
using SoftlarePMS.Application.Features.Employees.Commands.DeleteEmployee;
using SoftlarePMS.Application.Features.Employees.Commands.UpdateEmployee;
using SoftlarePMS.Application.Features.Employees.Queries.GetEmployeeById;
using SoftlarePMS.Application.Features.Employees.Queries.GetEmployeesWithPagination;
using SoftlarePMS.Domain.Enums;
using SoftlarePMS.WebApi.Authorization;

namespace SoftlarePMS.WebApi.Controllers;

[Authorize]
public sealed class EmployeesController : ApiControllerBase
{
    /// <summary>Get a paginated, filtered list of employees.</summary>
    /// <summary>Get a paginated, dynamically filtered list of employees.</summary>
    [HttpPost("search")]
    [HasPermission("Employees.Read")]
    [ProducesResponseType(typeof(PaginatedList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromBody] GetEmployeesWithPaginationQuery query,
        CancellationToken ct = default)
    {
        return Ok(await Sender.Send(query, ct));
    }

    /// <summary>Get a single employee with all related details.</summary>
    [HttpGet("{id:guid}")]
    [HasPermission("Employees.Read")]
    [ProducesResponseType(typeof(EmployeeDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await Sender.Send(new GetEmployeeByIdQuery(id), ct);
        return Ok(result);
    }

    /// <summary>Create a new employee with an initial address and compensation entry.</summary>
    [HttpPost]
    [HasPermission("Employees.Create")]
    [ProducesResponseType(typeof(CreatedEmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an employee's core profile fields.</summary>
    [HttpPut("{id:guid}")]
    [HasPermission("Employees.Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeCommand command, CancellationToken ct)
    {
        // Ensure the route id matches the command body
        if (id != command.EmployeeId)
            return BadRequest(new { message = "Route id does not match command EmployeeId." });

        await Sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>Soft-delete an employee.</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission("Employees.Delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await Sender.Send(new DeleteEmployeeCommand(id), ct);
        return NoContent();
    }
}
