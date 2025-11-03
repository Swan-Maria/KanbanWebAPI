using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<Team>> GetUserTeamsAsync(Guid userId);
}