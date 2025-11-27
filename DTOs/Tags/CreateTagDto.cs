namespace KanbanWebAPI.Application.DTOs.Tags;

public class CreateTagDto
{
    public string Name { get; set; } = default!;
    public string Color { get; set; } = default!;
}
