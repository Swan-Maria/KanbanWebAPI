using AutoMapper;
using KanbanWebAPI.Application.DTOs.Teams;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Domain.Repositories;
using KanbanWebAPI.Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class TeamService(ITeamRepository teamRepository, IMapper mapper) : ITeamService
{
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TeamDto>> GetUserTeamsAsync(Guid userId)
    {
        var teams = await _teamRepository.GetTeamsByUserAsync(userId);
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<TeamDto> CreateAsync(CreateTeamDto dto)
    {
        var entity = _mapper.Map<Team>(dto);
        await _teamRepository.CreateAsync(entity);
        return _mapper.Map<TeamDto>(entity);
    }

    public async Task<TeamDto?> UpdateAsync(Guid teamId, UpdateTeamDto dto)
    {
        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team is null) return null;

        _mapper.Map(dto, team);
        await _teamRepository.UpdateAsync(team);
        return _mapper.Map<TeamDto>(team);
    }

    public async Task DeleteAsync(Guid teamId)
    {
        await _teamRepository.DeleteByIdAsync(teamId);
    }
}
