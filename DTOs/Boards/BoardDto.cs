namespace KanbanWebAPI.Application.DTOs.Boards;

public class BoardDto
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
}
