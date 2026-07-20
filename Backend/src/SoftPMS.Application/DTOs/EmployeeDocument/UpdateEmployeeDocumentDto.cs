using SoftPMS.Domain.Enums;

namespace SoftPMS.Application.DTOs.EmployeeDocument;

public record UpdateEmployeeDocumentDto(
    string FileName,
    string FilePath,
    DocumentType DocumentType,
    DateTime? IssueDate,
    DateTime? ExpiryDate,
    DateTime? ReminderDate
);
