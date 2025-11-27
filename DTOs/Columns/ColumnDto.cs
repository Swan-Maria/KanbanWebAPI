namespace KanbanWebAPI.Application.DTOs.Columns;

public class ColumnDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = default!;
    public int Position { get; set; }
}
