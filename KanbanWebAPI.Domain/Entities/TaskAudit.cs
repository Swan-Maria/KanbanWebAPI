namespace KanbanWebAPI.Domain.Entities;

public class TaskAudit
{
    public Guid AuditId { get; set; }
    public Guid TaskId { get; set; }
    public TaskItem TaskItem { get; set; } = null!;
    public string ChangeDescription { get; set; } = null!;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public Guid ChengedByUserId { get; set; }
    public User ChengedByUser { get; set; } = null!;
}