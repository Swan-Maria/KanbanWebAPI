using KanbanWebAPI.Application.DTOs.Auth;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.DTOs.Users;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponse> SignInWithGoogleAsync(GoogleSignInRequest request)
    {
        // Тут ти підключиш реальну валідацію Google IdToken
        // Поки що — просто шукаємо користувача по GoogleId

        var googleId = request.IdToken; // тимчасово

        var user = await _userRepository.GetByGoogleIdAsync(googleId);
        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                GoogleId = googleId,
                Email = $"user_{Guid.NewGuid():N}@example.com",
                Name = "New User"
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        // Тут ти згенеруєш справжні JWT токени
        return new AuthResponse
        {
            AccessToken = "ACCESS_TOKEN_PLACEHOLDER",
            RefreshToken = "REFRESH_TOKEN_PLACEHOLDER",
            User = new UserDto
            {
                Id = user.Id,
                GoogleId = user.GoogleId,
                Email = user.Email,
                Name = user.Name,
                AvatarUrl = user.AvatarUrl
            }
        };
    }
}
