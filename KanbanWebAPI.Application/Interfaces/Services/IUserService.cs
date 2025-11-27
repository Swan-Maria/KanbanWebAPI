using KanbanWebAPI.Application.DTOs.Users;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid userId);
    Task<IEnumerable<UserDto>> GetAllAsync();
}
