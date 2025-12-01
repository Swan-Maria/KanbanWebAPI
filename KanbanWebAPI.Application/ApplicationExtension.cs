using AutoMapper;
using KanbanWebAPI.Application.Mapping;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanWebAPI.Application
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(KanbanProfile).Assembly);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IColumnService, ColumnService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskAuditService, TaskAuditService>();

            return services;
        }
    }
}
