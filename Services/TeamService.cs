using AutoMapper;
using KanbanWebAPI.Application.DTOs.Teams;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<List<TeamDto>> GetForCurrentUserAsync(Guid currentUserId)
    {
        var teams = await _teamRepository.GetForUserAsync(currentUserId);
        return _mapper.Map<List<TeamDto>>(teams);
    }

    public async Task<TeamDto> CreateAsync(Guid ownerId, CreateTeamDto dto)
    {
        var entity = _mapper.Map<Team>(dto);
        entity.Id = Guid.NewGuid();
        entity.OwnerId = ownerId;

        await _teamRepository.AddAsync(entity);
        await _teamRepository.SaveChangesAsync();

        return _mapper.Map<TeamDto>(entity);
    }
}
