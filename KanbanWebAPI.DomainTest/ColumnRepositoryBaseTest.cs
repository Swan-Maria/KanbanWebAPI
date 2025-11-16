using System.Text.Json;

using KanbanWebAPI.Domain;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.DomainTest;

[TestFixture]
public class ColumnRepositoryBaseTest
{
    private AppDbContext _context = null!;
    private ColumnRepositoryBase _columnRepositoryBase = null!;

    private const string Board1IdString = "00000000-0000-0000-0000-000000000100";
    private const string Board2IdString = "00000000-0000-0000-0000-000000000200";
    private const string NonExistentIdString = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";

    private static readonly List<Column> InitialColumns = LoadColumnsFromJson();

    private static List<Column> LoadColumnsFromJson()
    {
        var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestColumns.json");
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");
        }

        var jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<Column>>(jsonString) ?? new List<Column>();
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _columnRepositoryBase = new ColumnRepositoryBase(_context);

        _context.Columns.AddRange(InitialColumns.Select(c => new Column
        {
            ColumnId = c.ColumnId,
            BoardId = c.BoardId,
            ColumnName = c.ColumnName,
            Tasks = c.Tasks?.Select(t => new TaskItem { TaskId = t.TaskId, Title = t.Title, ColumnId = t.ColumnId })
                .ToList()
        }));

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private const string GetByBoardIdAsyncMethodName = nameof(ColumnRepositoryBase.GetByBoardIdAsync);

    // GetByBoardIdAsync
    [TestCase(Board1IdString, 2,
        Description = "Returns columns with tasks for a valid board ID 1",
        TestName = GetByBoardIdAsyncMethodName + " With Columns Board1")]
    [TestCase(Board2IdString, 1,
        Description = "Returns columns with tasks for a valid board ID 2",
        TestName = GetByBoardIdAsyncMethodName + " With Columns Board2")]
    [TestCase(NonExistentIdString, 0,
        Description = "Returns empty list when board ID has no columns or doesn't exist",
        TestName = GetByBoardIdAsyncMethodName + " Empty List")]
    public async Task GetByBoardIdAsync(string boardIdString, int expectedColumnCount)
    {
        // Arrange
        var boardId = Guid.Parse(boardIdString);

        // Act
        var result = await _columnRepositoryBase.GetByBoardIdAsync(boardId);

        // Assert 
        Assert.That(result.Count(), Is.EqualTo(expectedColumnCount),
            $"Expected {expectedColumnCount} columns for board {boardId}.");
    }
}