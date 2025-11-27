namespace KanbanWebAPI.Application.DTOs.Teams;

public class CreateTeamDto
{
    public string Name { get; set; } = default!;
    public bool IsPersonal { get; set; }
}
