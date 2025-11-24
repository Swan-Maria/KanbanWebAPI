using System.Text.Json;

using KanbanWebAPI.Domain;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.DomainTest;

[TestFixture]
public class TaskItemRepositoryBaseTest
{
    private AppDbContext _context = null!;
    private TaskItemRepositoryBase _taskItemRepositoryBase = null!;

    private const string Column1IdString = "00000000-0000-0000-0000-000000000100";
    private const string Column2IdString = "00000000-0000-0000-0000-000000000200";
    private const string NonExistentIdString = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";

    private static readonly List<TaskItem> InitialTasks = LoadTasksFromJson();

    private static List<TaskItem> LoadTasksFromJson()
    {
        var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestTaskItems.json");
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");
        }

        var jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<TaskItem>>(jsonString) ?? new List<TaskItem>();
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _taskItemRepositoryBase = new TaskItemRepositoryBase(_context);

        _context.Task.AddRange(InitialTasks.Select(t => new TaskItem
        {
            TaskId = t.TaskId,
            ColumnId = t.ColumnId,
            Title = t.Title,
            Description = t.Description
        }));

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private const string GetByColumnIdAsyncMethodName = nameof(ITaskItemRepository.GetByColumnIdAsync);

    // GetByColumnIdAsync Test Scenarios
    [TestCase(Column1IdString, 2, Description = "Returns tasks for existing column ID 1",
        TestName = GetByColumnIdAsyncMethodName + " With Tasks 1")]
    [TestCase(Column2IdString, 1, Description = "Returns tasks for existing column ID 2",
        TestName = GetByColumnIdAsyncMethodName + " With Tasks 2")]
    [TestCase(NonExistentIdString, 0, Description = "Returns empty list when column ID doesn't exist",
        TestName = GetByColumnIdAsyncMethodName + " Empty List")]
    public async Task GetByColumnIdAsync(string columnIdString, int expectedCount)
    {
        // Arrange
        var columnId = Guid.Parse(columnIdString);

        // Act
        var result = await _taskItemRepositoryBase.GetByColumnIdAsync(columnId);
        var resultList = result.ToList();

        if (expectedCount == 0)
        {
            Assert.That(resultList, Is.Empty, "The list should be empty for a non-existent column.");
        }
        else
        {
            Assert.That(resultList.Count(), Is.EqualTo(expectedCount),
                "The count of tasks should match the expected count.");
            Assert.That(resultList.All(t => t.ColumnId == columnId), Is.True,
                "All returned tasks must belong to the specified ColumnId.");
        }
    }
}