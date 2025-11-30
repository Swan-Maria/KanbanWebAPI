namespace KanbanWebAPI.Application.DTOs.Tasks;

public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ColumnId { get; set; }
}