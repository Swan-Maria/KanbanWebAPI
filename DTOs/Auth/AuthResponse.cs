using KanbanWebAPI.Application.DTOs.Users;

namespace KanbanWebAPI.Application.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public UserDto User { get; set; } = default!;
}
