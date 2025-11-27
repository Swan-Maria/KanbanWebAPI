using KanbanWebAPI.Application.DTOs.Teams;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITeamService
{
    Task<List<TeamDto>> GetForCurrentUserAsync(Guid currentUserId);
    Task<TeamDto> CreateAsync(Guid ownerId, CreateTeamDto dto);
}
