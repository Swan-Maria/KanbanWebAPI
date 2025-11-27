using AutoMapper;
using KanbanWebAPI.Application.DTOs.Users;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;

namespace KanbanWebAPI.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }
}
