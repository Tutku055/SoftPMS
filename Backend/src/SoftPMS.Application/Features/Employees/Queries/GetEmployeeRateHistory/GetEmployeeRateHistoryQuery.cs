using MediatR;

namespace SoftPMS.Application.Features.Employees.Queries.GetEmployeeRateHistory;

/// <summary>Query to retrieve the complete compensation (rate/Stundenlohn) history for a given employee, ordered newest-first.</summary>
public sealed record GetEmployeeRateHistoryQuery(Guid EmployeeId) : IRequest<IReadOnlyList<CompensationHistoryDto>>;
