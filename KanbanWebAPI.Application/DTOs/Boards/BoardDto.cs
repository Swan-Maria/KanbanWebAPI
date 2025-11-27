namespace KanbanWebAPI.Application.DTOs.Boards;

public class BoardDto
{
    public Guid BoardId { get; set; }
    public string BoardName { get; set; } = null!;
    public string? BoardDescription { get; set; }
    public Guid TeamId { get; set; }
}
