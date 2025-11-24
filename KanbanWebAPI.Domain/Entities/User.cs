namespace KanbanWebAPI.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string GoogleId { get; init; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IReadOnlyCollection<Team> Teams { get; set; } = new List<Team>();
    public IReadOnlyCollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public IReadOnlyCollection<TaskAudit> TaskAudits { get; set; } = new List<TaskAudit>();
}