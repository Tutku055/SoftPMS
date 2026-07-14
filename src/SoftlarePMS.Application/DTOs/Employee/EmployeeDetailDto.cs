using SoftlarePMS.Domain.Enums;
using SoftlarePMS.Application.DTOs.EmployeeAddress;
using SoftlarePMS.Application.DTOs.EmployeeCompensation;
using SoftlarePMS.Application.DTOs.EmployeeDocument;
using SoftlarePMS.Application.DTOs.EmployeeNote;
using SoftlarePMS.Application.DTOs.EmployeeReference;

namespace SoftlarePMS.Application.DTOs.Employee;

public record EmployeeDetailDto(
    Guid Id,
    string EmployeeNo,
    string FirstName,
    string LastName,
    Gender Gender,
    DateTime DateOfBirth,
    string Nationality,
    string Profession,
    EmploymentStatus EmploymentStatus,
    DateTime HireDate,
    DateTime? TerminationDate,
    DateTime? ProbationEndDate,
    decimal WorkingHoursPerWeek,
    int VacationDaysTotal,

    List<EmployeeAddressDto> Addresses,
    List<EmployeeCompensationDto> Compensations,
    List<EmployeeDocumentDto> Documents,
    List<EmployeeNoteDto> Notes,
    List<EmployeeReferenceDto> References
);