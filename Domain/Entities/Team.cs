namespace Domain.Entities;

public class Team
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Board> Boards { get; set; } = new List<Board>();
}