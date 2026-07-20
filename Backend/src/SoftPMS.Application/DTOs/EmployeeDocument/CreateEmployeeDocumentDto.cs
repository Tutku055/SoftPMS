using SoftPMS.Domain.Enums;

namespace SoftPMS.Application.DTOs.EmployeeDocument;

public record CreateEmployeeDocumentDto(
    string FileName,
    string FilePath,
    DocumentType DocumentType
);