using System.Text.Json;

using KanbanWebAPI.Domain;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.DomainTest;

[TestFixture]
public class BoardRepositoryBaseTests
{
    private AppDbContext _context = null!;
    private BoardRepositoryBase _boardRepositoryBase = null!;

    private const string BoardAlphaIdString = "00000000-0000-0000-0000-000000000101";
    private const string TeamAIdString = "00000000-0000-0000-0000-000000000001";
    private const string TeamBIdString = "00000000-0000-0000-0000-000000000002";
    private const string NonExistentIdString = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";

    private static readonly List<Board> InitialBoards = LoadBoardsFromJson();

    private static List<Board> LoadBoardsFromJson()
    {
        var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestBoards.json");
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");
        }

        var jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<Board>>(jsonString) ?? new List<Board>();
    }

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _context.Board.AddRange(InitialBoards);
        _context.SaveChanges();

        _boardRepositoryBase = new BoardRepositoryBase(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private const string GetAllAsyncMethodName = nameof(IRepositoryBase<Board>.GetAllAsync);
    private const string GetByIdAsyncMethodName = nameof(IRepositoryBase<Board>.GetByIdAsync);
    private const string CreateAsyncMethodName = nameof(IRepositoryBase<Board>.CreateAsync);
    private const string UpdateAsyncMethodName = nameof(IRepositoryBase<Board>.UpdateAsync);
    private const string DeleteByIdAsyncMethodName = nameof(IRepositoryBase<Board>.DeleteByIdAsync);
    private const string GetByNameAsyncMethodName = nameof(IBoardRepository.GetByNameAsync);
    private const string GetByTeamIdAsyncMethodName = nameof(IBoardRepository.GetByTeamIdAsync);


    // GetAllAsync 
    [TestCase(3, Description = "Returns all boards when multiple exist",
        TestName = GetAllAsyncMethodName + " With Data")]
    public async Task GetAllAsync(int expectedCount)
    {
        // Arrange (SetUp)

        // Act
        var result = await _boardRepositoryBase.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(expectedCount),
            "The count of returned boards should match the number of boards in JSON.");
    }

    // GetByIdAsync
    [TestCase(BoardAlphaIdString, true, Description = "Get existing board by ID",
        TestName = GetByIdAsyncMethodName + " With Existing Id")]
    [TestCase(NonExistentIdString, false, Description = "Get non-existing board by ID",
        TestName = GetByIdAsyncMethodName + " With Non Existing Id")]
    public async Task GetByIdAsync(string idString, bool expectSuccess)
    {
        // Arrange
        Guid testId = Guid.Parse(idString);

        // Act
        var result = await _boardRepositoryBase.GetByIdAsync(testId);

        // Assert
        if (expectSuccess)
        {
            Assert.That(result!.BoardId, Is.EqualTo(testId),
                "The retrieved board ID must match the requested existing ID.");
        }
        else
        {
            Assert.That(result, Is.Null, "Result should be null for a non-existent ID.");
        }
    }

    // CreateAsync
    [TestCase("00000000-0000-0000-0000-000000000999", "New Board Name", null,
        Description = "Create with valid data", TestName = CreateAsyncMethodName + " With Data")]
    [TestCase(null, null, typeof(ArgumentNullException),
        Description = "Create with null entity", TestName = CreateAsyncMethodName + " With Null Entity")]
    public async Task CreateAsync(string? boardIdString, string? boardName, Type? expectedExceptionType)
    {
        // Arrange
        Board? board = null;
        if (boardIdString != null && boardName != null)
        {
            board = new Board { BoardId = Guid.Parse(boardIdString), TeamId = Guid.NewGuid(), BoardName = boardName };
        }

        // Act & Assert
        if (expectedExceptionType != null)
        {
            Assert.ThrowsAsync(expectedExceptionType, async () => await _boardRepositoryBase.CreateAsync(board!));
        }
        else
        {
            // Act
            await _boardRepositoryBase.CreateAsync(board!);
            var savedBoard = await _context.Board.FindAsync(board!.BoardId);

            // Assert (1 assert)
            Assert.That(savedBoard!.BoardName, Is.EqualTo(boardName),
                "The saved board name should match the input name.");
        }
    }

    // UpdateAsync
    [TestCase(BoardAlphaIdString, "Updated Alpha Name", null,
        Description = "Update existing board successfully", TestName = UpdateAsyncMethodName + " Success")]
    [TestCase(NonExistentIdString, null, typeof(DbUpdateConcurrencyException),
        Description = "Update non-existent entity throws concurrency exception",
        TestName = UpdateAsyncMethodName + " Concurrency Conflict")]
    public async Task UpdateAsync(string idString, string? newName, Type? expectedExceptionType)
    {
        // Arrange
        Guid boardId = Guid.Parse(idString);
        Board boardToUpdate;

        if (expectedExceptionType == null)
        {
            boardToUpdate = _context.Board.First(b => b.BoardId == boardId);
            boardToUpdate.BoardName = newName!;
        }
        else
        {
            boardToUpdate = new Board { BoardId = boardId, BoardName = "Fake" };
        }

        // Act & Assert
        if (expectedExceptionType != null)
        {
            Assert.ThrowsAsync(expectedExceptionType, async () => await _boardRepositoryBase.UpdateAsync(boardToUpdate));
        }
        else
        {
            // Act
            await _boardRepositoryBase.UpdateAsync(boardToUpdate);
            var updated = await _context.Board.AsNoTracking().FirstOrDefaultAsync(b => b.BoardId == boardId);

            // Assert
            Assert.That(updated!.BoardName, Is.EqualTo(newName), "The board name was not updated correctly.");
        }
    }


    // DeleteByIdAsync
    [TestCase(BoardAlphaIdString, Description = "Delete existing board by ID",
        TestName = DeleteByIdAsyncMethodName + " Success")]
    [TestCase(NonExistentIdString, Description = "Delete non-existing board by ID (does nothing)",
        TestName = DeleteByIdAsyncMethodName + " Not Found")]
    public async Task DeleteByIdAsync(string idString)
    {
        // Arrange
        Guid boardIdToDelete = Guid.Parse(idString);

        // Act
        await _boardRepositoryBase.DeleteByIdAsync(boardIdToDelete);
        var deleted = await _context.Board.FindAsync(boardIdToDelete);

        // Assert 
        Assert.That(deleted, Is.Null,
            "The entity should be null after successful deletion or if it didn't exist.");
    }

    // GetByNameAsync
    [TestCase("Project Alpha", true, Description = "Get board by existing name",
        TestName = GetByNameAsyncMethodName + " With Existing Name")]
    [TestCase("Non-Existent Board", false, Description = "Get board by non-existing name",
        TestName = GetByNameAsyncMethodName + " With Non Existing Name")]
    public async Task GetByNameAsync(string boardName, bool shouldExist)
    {
        // Arrange (SetUp)

        // Act
        var result = await _boardRepositoryBase.GetByNameAsync(boardName);

        // Assert
        if (shouldExist)
        {
            Assert.That(result?.BoardName, Is.EqualTo(boardName),
                "Should retrieve the correct board when name exists.");
        }
        else
        {
            Assert.That(result, Is.Null, "Should return null when name does not exist.");
        }
    }

    // GetByTeamIdAsync
    [TestCase(TeamAIdString, 2, Description = "Get boards for team A (2 boards)",
        TestName = GetByTeamIdAsyncMethodName + " Team A")]
    [TestCase(TeamBIdString, 1, Description = "Get boards for team B (1 board)",
        TestName = GetByTeamIdAsyncMethodName + " Team B")]
    [TestCase(NonExistentIdString, 0, Description = "Get boards for non-existent team",
        TestName = GetByTeamIdAsyncMethodName + " Non Existent Team")]
    public async Task GetByTeamIdAsync(string teamIdString, int expectedCount)
    {
        // Arrange
        var teamId = new Guid(teamIdString);

        // Act
        var result = await _boardRepositoryBase.GetByTeamIdAsync(teamId);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(expectedCount),
            $"Should return {expectedCount} boards for the given team ID.");
    }
}