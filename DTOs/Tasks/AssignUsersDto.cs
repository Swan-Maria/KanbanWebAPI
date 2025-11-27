namespace KanbanWebAPI.Application.DTOs.Tasks;

public class AssignUsersDto
{
    public List<Guid> UserIds { get; set; } = new();
}
