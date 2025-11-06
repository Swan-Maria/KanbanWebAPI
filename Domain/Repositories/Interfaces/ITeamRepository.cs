using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface ITeamRepository : IGenericRepository<Team>
{
    Task<IEnumerable<User>> GetTeamUsersAsync(Guid teamId);
}