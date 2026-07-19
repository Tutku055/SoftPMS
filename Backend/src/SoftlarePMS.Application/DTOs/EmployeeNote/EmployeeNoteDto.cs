namespace SoftlarePMS.Application.DTOs.EmployeeNote;

public record EmployeeNoteDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Content { get; init; }
    public DateTime CreatedAt { get; init; }
}