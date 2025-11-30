using AutoMapper;

using KanbanWebAPI.Application.DTOs.Boards;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

namespace KanbanWebAPI.Application.Services;

public class BoardService(IBoardRepository boardRepository, IMapper mapper) : IBoardService
{
    private readonly IBoardRepository _boardRepository = boardRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<BoardDto>> GetByTeamAsync(Guid teamId)
    {
        var boards = await _boardRepository.GetByTeamIdAsync(teamId);
        return _mapper.Map<IEnumerable<BoardDto>>(boards);
    }

    public async Task<BoardDto?> GetByIdAsync(Guid boardId)
    {
        var board = await _boardRepository.GetByIdAsync(boardId);
        return board is null ? null : _mapper.Map<BoardDto>(board);
    }

    public async Task<BoardDto> CreateAsync(CreateBoardDto dto)
    {
        var entity = _mapper.Map<Board>(dto);
        await _boardRepository.CreateAsync(entity);
        return _mapper.Map<BoardDto>(entity);
    }

    public async Task<BoardDto?> UpdateAsync(Guid boardId, UpdateBoardDto dto)
    {
        var entity = await _boardRepository.GetByIdAsync(boardId);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _boardRepository.UpdateAsync(entity);
        return _mapper.Map<BoardDto>(entity);
    }

    public async Task DeleteAsync(Guid boardId)
    {
        await _boardRepository.DeleteByIdAsync(boardId);
    }
}