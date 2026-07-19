using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Application.DTOs.EmployeeDocument;

public record EmployeeDocumentDto
{
    public Guid Id { get; init; }
    public string FileName { get; init; }
    public string FilePath { get; init; }
    public DocumentType DocumentType { get; init; }
    public DateTime UploadedAt { get; init; }
}