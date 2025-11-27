using KanbanWebAPI.Application.DTOs.Users;

namespace KanbanWebAPI.Application.DTOs.Tasks;

public class TaskAssigneeDto
{
    public Guid UserId { get; set; }
    public UserDto User { get; set; } = default!;
}
