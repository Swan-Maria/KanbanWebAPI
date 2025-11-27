namespace KanbanWebAPI.Application.DTOs.Columns;

public class ColumnDto
{
    public Guid ColumnId { get; set; }
    public string ColumnName { get; set; } = null!;
    public Guid BoardId { get; set; }
}
