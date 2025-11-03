using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Implementations;

internal class TaskItemRepository : GenericRepository<TaskItem>, ITaskRepository
{
    public TaskItemRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<TaskItem>> GetByColumnIdAsync(Guid columnId)
    {
        return await _context.Tasks
            .Include(t => t.Users)
            .Where(t => t.ColumnId == columnId)
            .ToListAsync();
    }
}