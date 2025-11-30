namespace KanbanWebAPI.Domain.Entities;

public class TaskItem
{
    public Guid TaskId { get; init; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ColumnId { get; set; }
    public Column Column { get; set; } = null!;
    public IReadOnlyCollection<TaskAudit> TaskAudits { get; set; } = new List<TaskAudit>();
}