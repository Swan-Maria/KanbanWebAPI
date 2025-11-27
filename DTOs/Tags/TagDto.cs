namespace KanbanWebAPI.Application.DTOs.Tags;

public class TagDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = default!;
    public string Color { get; set; } = default!;
}
