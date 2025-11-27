namespace KanbanWebAPI.Application.DTOs.Tasks;

public class MoveTaskDto
{
    public Guid ToColumnId { get; set; }
    public int? Position { get; set; }
}
