namespace KanbanWebAPI.Application.DTOs.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string GoogleId { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? AvatarUrl { get; set; }
}
