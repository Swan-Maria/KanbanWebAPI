using KanbanWebAPI.Domain.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace KanbanWebAPI.Domain;

public static class DomainExtension
{
    public static void RegisterDomain(this IServiceCollection services)
    {
        services.AddScoped<IBoardRepository, BoardRepositoryBase>();
        services.AddScoped<IColumnRepository, ColumnRepositoryBase>();
        services.AddScoped<ITaskAuditRepository, TaskAuditRepositoryBase>();
        services.AddScoped<ITaskItemRepository, TaskItemRepositoryBase>();
        services.AddScoped<ITeamRepository, TeamRepositoryBase>();
        services.AddScoped<IUserRepository, UserRepositoryBase>();
    }
}