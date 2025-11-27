using KanbanWebAPI.Application.DTOs.Teams;

namespace KanbanWebAPI.Application.Interfaces.Services;

public interface ITeamService
{
    Task<IEnumerable<TeamDto>> GetUserTeamsAsync(Guid userId);
    Task<TeamDto> CreateAsync(CreateTeamDto dto);
    Task<TeamDto?> UpdateAsync(Guid teamId, UpdateTeamDto dto);
    Task DeleteAsync(Guid teamId);
}
