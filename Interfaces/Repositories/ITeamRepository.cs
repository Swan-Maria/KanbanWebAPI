using Domain.Entities;

namespace KanbanWebAPI.Application.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<List<Team>> GetForUserAsync(Guid userId);
    Task AddAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(Team team);
    Task SaveChangesAsync();
}
