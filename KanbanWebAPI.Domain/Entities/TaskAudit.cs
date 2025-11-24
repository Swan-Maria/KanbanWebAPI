namespace KanbanWebAPI.Domain.Entities;

public class TaskAudit
{
    public Guid AuditId { get; init; }
    public Guid TaskItemId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;
    public string Action { get; set; } = null!;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public Guid CreateByUserId { get; set; }
    public User CreateByUser { get; set; } = null!;
}