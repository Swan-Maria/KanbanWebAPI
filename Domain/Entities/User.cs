namespace Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string GoogleId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<TaskAudit> TaskAudits { get; set; } = new List<TaskAudit>();
}