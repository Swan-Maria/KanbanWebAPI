using System.Text.Json;

using AutoMapper;

using KanbanWebAPI.Application.DTOs.Boards;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Services;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Moq;


namespace KanbanWebAPI.ApplicationTest.Services;

[TestFixture]
public class BoardServiceTests
{
    private Mock<IBoardRepository> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private BoardService _service;

    private static List<Board> LoadBoardsFromJson()
    {
        var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "TestBoards.json");
        if (!File.Exists(jsonFilePath))
            throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");

        var jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<Board>>(jsonString) ?? new List<Board>();
    }

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<IBoardRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new BoardService(_mockRepo.Object, _mockMapper.Object);
    }

    private const string GetByIdAsyncMethodName = nameof(IBoardService.GetByIdAsync);
    private const string GetByTeamAsyncMethodName = nameof(IBoardService.GetByTeamAsync);
    private const string UpdateAsyncMethodName = nameof(IBoardService.UpdateAsync);
    private const string DeleteAsyncMethodName = nameof(IBoardService.DeleteAsync);

    [Test]
    [TestCase("11111111-1111-1111-1111-111111111111", true, Description = "Existing board ID",
        TestName = GetByIdAsyncMethodName + " With existing board  ID")]
    [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, Description = "Non-existing board ID",
        TestName = GetByIdAsyncMethodName + " With non-existing board  ID")]
    public async Task GetByIdAsync(string boardIdStr, bool exists)
    {
        var boardId = Guid.Parse(boardIdStr);
        var board = LoadBoardsFromJson().FirstOrDefault(b => b.BoardId == boardId);

        _mockRepo.Setup(r => r.GetByIdAsync(boardId)).ReturnsAsync(board);
        _mockMapper.Setup(m => m.Map<BoardDto>(It.IsAny<Board>()))
            .Returns((Board b) => new BoardDto { BoardId = b.BoardId, BoardName = b.BoardName });

        var result = await _service.GetByIdAsync(boardId);

        Assert.That(result != null, Is.EqualTo(exists));
    }

    [Test]
    [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 2, Description = "Team A has 2 boards",
        TestName = GetByTeamAsyncMethodName + " With existing team ID")]
    [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", 0, Description = "Non-existing team ID",
        TestName = GetByTeamAsyncMethodName + " With non-existing team ID")]
    public async Task GetByTeamAsync(string teamIdStr, int expectedCount)
    {
        var teamId = Guid.Parse(teamIdStr);
        var boards = LoadBoardsFromJson().Where(b => b.TeamId == teamId).ToList();

        _mockRepo.Setup(r => r.GetByTeamIdAsync(teamId)).ReturnsAsync(boards);
        _mockMapper.Setup(m => m.Map<IEnumerable<BoardDto>>(It.IsAny<IEnumerable<Board>>()))
            .Returns((IEnumerable<Board> b) =>
                b.Select(x => new BoardDto { BoardId = x.BoardId, BoardName = x.BoardName }));

        var result = await _service.GetByTeamAsync(teamId);

        Assert.That(result.Count(), Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task CreateAsync()
    {
        var createDto = new CreateBoardDto
        {
            BoardName = "New Board",
            TeamId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
        };
        var entity = new Board
        {
            BoardId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            BoardName = createDto.BoardName,
            TeamId = createDto.TeamId
        };

        _mockMapper.Setup(m => m.Map<Board>(It.IsAny<CreateBoardDto>())).Returns(entity);
        _mockRepo.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
        _mockMapper.Setup(m => m.Map<BoardDto>(entity)).Returns(new BoardDto
        {
            BoardId = entity.BoardId,
            BoardName = entity.BoardName
        });

        var result = await _service.CreateAsync(createDto);

        Assert.That(result.BoardId, Is.EqualTo(entity.BoardId));
        Assert.That(result.BoardName, Is.EqualTo(entity.BoardName));
    }


    [Test]
    [TestCase("11111111-1111-1111-1111-111111111111", true, Description = "Existing board to update",
        TestName = UpdateAsyncMethodName + " With existing board  ID")]
    [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, Description = "Non-existing board to update",
        TestName = UpdateAsyncMethodName + " With non-existing board  ID")]
    public async Task UpdateAsync(string boardIdStr, bool exists)
    {
        var boardId = Guid.Parse(boardIdStr);
        var dto = new UpdateBoardDto { BoardName = "Updated Name" };
        var board = LoadBoardsFromJson().FirstOrDefault(b => b.BoardId == boardId);

        _mockRepo.Setup(r => r.GetByIdAsync(boardId)).ReturnsAsync(board);
        if (board != null)
            _mockRepo.Setup(r => r.UpdateAsync(board)).Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map(dto, board ?? new Board()));
        _mockMapper.Setup(m => m.Map<BoardDto>(It.IsAny<Board>()))
            .Returns((Board b) => new BoardDto { BoardId = b.BoardId, BoardName = b.BoardName });

        var result = await _service.UpdateAsync(boardId, dto);

        Assert.That(result != null, Is.EqualTo(exists));
    }

    [Test]
    [TestCase("11111111-1111-1111-1111-111111111111", TestName = DeleteAsyncMethodName + " With existing board  ID")]
    [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF",
        TestName = DeleteAsyncMethodName + " With non-existing board  ID")]
    public async Task DeleteAsync(string boardIdStr)
    {
        var boardId = Guid.Parse(boardIdStr);

        _mockRepo.Setup(r => r.DeleteByIdAsync(boardId)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(boardId);

        _mockRepo.Verify(r => r.DeleteByIdAsync(boardId), Times.Once);
    }
}