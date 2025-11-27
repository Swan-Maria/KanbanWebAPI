namespace KanbanWebAPI.Application.DTOs.Columns;

public class CreateColumnDto
{
    public string ColumnName { get; set; } = null!;
    public Guid BoardId { get; set; }
}
