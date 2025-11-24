namespace KanbanWebAPI.Domain.Entities;

public class Column
{
    public Guid ColumnId { get; init; }
    public string ColumnName { get; set; } = null!;
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public IReadOnlyCollection<TaskItem>? Tasks { get; set; } = new List<TaskItem>();
}