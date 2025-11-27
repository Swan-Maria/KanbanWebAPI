namespace KanbanWebAPI.Application.DTOs.Boards;

public class CreateBoardDto
{
    public Guid TeamId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
