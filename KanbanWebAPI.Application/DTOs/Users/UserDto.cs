namespace KanbanWebAPI.Application.DTOs.Users;

public class UserDto
{
    public Guid UserId { get; set; }
    public string GoogleId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}