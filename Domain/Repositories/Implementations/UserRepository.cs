using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Implementations;

internal class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Team>> GetUserTeamsAsync(Guid userId)
    {
        return await _context.Teams
            .Where(t => t.Users.Any(u => u.UserId == userId))
            .ToListAsync();
    }
}