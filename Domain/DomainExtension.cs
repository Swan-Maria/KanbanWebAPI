using Domain.Repositories;
using Domain.Repositories.Implementations;
using Domain.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DomainExtension
{
    public static void RegisterDomain(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IColumnRepository, ColumnRepository>();
        services.AddScoped<ITaskAuditRepository, TaskAuditRepository>();
        services.AddScoped<ITaskRepository, TaskItemRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}