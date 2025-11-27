namespace KanbanWebAPI.Application.DTOs.Tasks;

public class TaskDto
{
    public Guid Id { get; set; }
    public Guid ColumnId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int? Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
}
