using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.DTOs.EmployeeDocument;

public record CreateEmployeeDocumentDto(
    string FileName,
    string FilePath,
    DocumentType DocumentType
);