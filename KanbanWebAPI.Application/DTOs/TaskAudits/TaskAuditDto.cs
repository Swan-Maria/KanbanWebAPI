namespace KanbanWebAPI.Application.DTOs.TaskAudits;

public class TaskAuditDto
{
    public Guid AuditId { get; set; }
    public Guid TaskItemId { get; set; }
    public string Action { get; set; } = null!;
    public DateTime CreateAt { get; set; }
    public Guid CreateByUserId { get; set; }
}
