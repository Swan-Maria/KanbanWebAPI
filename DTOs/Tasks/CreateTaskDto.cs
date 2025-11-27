namespace KanbanWebAPI.Application.DTOs.Tasks;

public class CreateTaskDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int? Priority { get; set; }
    public DateTime? DueDate { get; set; }
}
