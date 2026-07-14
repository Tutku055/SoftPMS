namespace SoftlarePMS.Application.DTOs.EmployeeNote;

public record EmployeeNoteDto(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAt
);