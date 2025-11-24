using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain.Repositories;

public interface ITeamRepository : IRepositoryBase<Team>
{
    Task<IEnumerable<Team>> GetTeamsByUserAsync(Guid userId);
}

internal class TeamRepositoryBase(AppDbContext context) : RepositoryBase<Team>(context), ITeamRepository
{

    public async Task<IEnumerable<Team>> GetTeamsByUserAsync(Guid userId)
    {
        return await _context.Team
            .Where(t => t.Users.Any(u => u.UserId == userId))
            .ToListAsync();
    }
}