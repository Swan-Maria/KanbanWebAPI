using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByTeamAsync(Guid teamId);
}

internal class UserRepositoryBase(AppDbContext context) : RepositoryBase<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetUsersByTeamAsync(Guid teamId)
    {
        return await _context.User
            .Where(u => u.Teams.Any(t => t.TeamId == teamId))
            .ToListAsync();
    }
}