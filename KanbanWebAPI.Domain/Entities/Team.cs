namespace KanbanWebAPI.Domain.Entities;

public class Team
{
    public Guid TeamId { get; init; }
    public string TeamName { get; set; } = null!;
    public IReadOnlyCollection<User> Users { get; set; } = new List<User>();
    public IReadOnlyCollection<Board> Boards { get; set; } = new List<Board>();
}