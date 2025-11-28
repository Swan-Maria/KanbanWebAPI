using AutoMapper;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Mapping;
using KanbanWebAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanWebAPI.Application;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // AutoMapper з профілем KanbanProfile
        services.AddAutoMapper(typeof(KanbanProfile).Assembly);

        // Реєстрація сервісів
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IColumnService, ColumnService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITaskAuditService, TaskAuditService>();

        return services;
    }
}

