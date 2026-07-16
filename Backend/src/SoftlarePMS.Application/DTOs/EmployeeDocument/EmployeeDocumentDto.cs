using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.DTOs.EmployeeDocument;

public record EmployeeDocumentDto(
    Guid Id,
    string FileName,
    string FilePath,
    DocumentType DocumentType,
    DateTime UploadedAt
);