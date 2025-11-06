namespace Domain.Entities;

public class Column
{
    public Guid ColumnId { get; set; }
    public string ColumnName { get; set; } = null!;
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}