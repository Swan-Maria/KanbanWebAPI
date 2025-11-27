using AutoMapper;
using KanbanWebAPI.Application.DTOs.Boards;
using KanbanWebAPI.Application.Interfaces.Repositories;
using KanbanWebAPI.Application.Interfaces.Services;
using Domain.Entities;

namespace KanbanWebAPI.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;

    public BoardService(IBoardRepository boardRepository, IMapper mapper)
    {
        _boardRepository = boardRepository;
        _mapper = mapper;
    }

    public async Task<List<BoardDto>> GetForTeamAsync(Guid teamId)
    {
        var boards = await _boardRepository.GetForTeamAsync(teamId);
        return _mapper.Map<List<BoardDto>>(boards);
    }

    public async Task<BoardDto?> GetByIdAsync(Guid boardId)
    {
        var board = await _boardRepository.GetByIdAsync(boardId);
        return board is null ? null : _mapper.Map<BoardDto>(board);
    }

    public async Task<BoardDto> CreateAsync(CreateBoardDto dto)
    {
        var entity = _mapper.Map<Board>(dto);
        entity.Id = Guid.NewGuid();

        await _boardRepository.AddAsync(entity);
        await _boardRepository.SaveChangesAsync();

        return _mapper.Map<BoardDto>(entity);
    }

    public async Task<BoardDto?> UpdateAsync(Guid id, UpdateBoardDto dto)
    {
        var entity = await _boardRepository.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _boardRepository.UpdateAsync(entity);
        await _boardRepository.SaveChangesAsync();

        return _mapper.Map<BoardDto>(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _boardRepository.GetByIdAsync(id);
        if (entity is null) return;

        await _boardRepository.DeleteAsync(entity);
        await _boardRepository.SaveChangesAsync();
    }
}
