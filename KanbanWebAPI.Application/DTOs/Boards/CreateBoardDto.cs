namespace KanbanWebAPI.Application.DTOs.Boards;

public class CreateBoardDto
{
    public string BoardName { get; set; } = null!;
    public string? BoardDescription { get; set; }
    public Guid TeamId { get; set; }
}
