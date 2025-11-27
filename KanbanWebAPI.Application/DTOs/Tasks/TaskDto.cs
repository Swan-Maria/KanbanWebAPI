namespace KanbanWebAPI.Application.DTOs.Tasks;

public class TaskDto
{
    public Guid TaskId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ColumnId { get; set; }
}
