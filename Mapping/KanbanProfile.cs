using AutoMapper;
using Domain.Entities;
using KanbanWebAPI.Application.DTOs.Users;
using KanbanWebAPI.Application.DTOs.Teams;
using KanbanWebAPI.Application.DTOs.Boards;
using KanbanWebAPI.Application.DTOs.Columns;
using KanbanWebAPI.Application.DTOs.Tasks;
using KanbanWebAPI.Application.DTOs.Comments;
using KanbanWebAPI.Application.DTOs.Tags;

namespace KanbanWebAPI.Application.Mapping;

public class KanbanProfile : Profile
{
    public KanbanProfile()
    {
        // User
        CreateMap<User, UserDto>();

        // Team
        CreateMap<Team, TeamDto>();
        CreateMap<CreateTeamDto, Team>();
        CreateMap<UpdateTeamDto, Team>();

        // Board
        CreateMap<Board, BoardDto>();
        CreateMap<CreateBoardDto, Board>();
        CreateMap<UpdateBoardDto, Board>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Column
        CreateMap<Column, ColumnDto>();
        CreateMap<CreateColumnDto, Column>();
        CreateMap<UpdateColumnDto, Column>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Task
        CreateMap<TaskItem, TaskDto>();
        CreateMap<CreateTaskDto, TaskItem>();
        CreateMap<UpdateTaskDto, TaskItem>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Comment
        CreateMap<Comment, CommentDto>();

        // Tag
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>();
        CreateMap<UpdateTagDto, Tag>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
