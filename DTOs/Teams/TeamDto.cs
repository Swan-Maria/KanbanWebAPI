namespace KanbanWebAPI.Application.DTOs.Teams;

public class TeamDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsPersonal { get; set; }
    public Guid OwnerId { get; set; }
}
