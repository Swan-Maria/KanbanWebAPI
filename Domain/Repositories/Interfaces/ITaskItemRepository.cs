using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface ITaskRepository : IGenericRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetByColumnIdAsync(Guid columnId);
}