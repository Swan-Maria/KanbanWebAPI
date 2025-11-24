using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain.Repositories;

public interface ITaskItemRepository : IRepositoryBase<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetByColumnIdAsync(Guid columnId);
}

internal class TaskItemRepositoryBase(AppDbContext context) : RepositoryBase<TaskItem>(context), ITaskItemRepository
{
    public async Task<IEnumerable<TaskItem>> GetByColumnIdAsync(Guid columnId)
    {
        return await _context.Task
            .Include(t => t.Users)
            .Where(t => t.ColumnId == columnId)
            .ToListAsync();
    }
}