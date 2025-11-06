namespace Domain.Entities;

public class Board
{
    public Guid BoardId { get; set; }
    public string BoardName { get; set; } = null!;
    public string? BoardDescription { get; set; }
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public ICollection<Column> Columns { get; set; } = new List<Column>();
}