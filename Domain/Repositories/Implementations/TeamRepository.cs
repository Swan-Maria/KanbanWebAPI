using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Implementations;

internal class TeamRepository : GenericRepository<Team>, ITeamRepository
{
    public TeamRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<User>> GetTeamUsersAsync(Guid teamId)
    {
        return await _context.Users
            .Where(u => u.Teams.Any(t => t.TeamId == teamId))
            .ToListAsync();
    }
}