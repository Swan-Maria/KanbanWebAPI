using KanbanWebAPI.Application.DTOs.Auth;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> SignInWithGoogleAsync(GoogleSignInRequest request);
}
