using Domain;
using Domain.Entities;
using Domain.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DomainTest;

[TestFixture]
public class TaskItemRepositoryTest
{
    private AppDbContext _context = null!;
    private TaskItemRepository _repository = null!;
    private Guid _column1;
    private Guid _column2;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new TaskItemRepository(_context);

        _column1 = Guid.NewGuid();
        _column2 = Guid.NewGuid();

        _context.Tasks.AddRange(
            new TaskItem
            {
                TaskId = Guid.NewGuid(),
                ColumnId = _column1,
                Title = "Create data base"
            },
            new TaskItem
            {
                TaskId = Guid.NewGuid(),
                ColumnId = _column1,
                Title = "Create data access layer"
            },
            new TaskItem
            {
                TaskId = Guid.NewGuid(),
                ColumnId = _column2,
                Title = "Test data access layer"
            }
        );

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetByColumnIdAsync_ReturnsTasksForGivenColumn()
    {
        // Arrange
        var columnId = _column1;

        // Act
        var result = await _repository.GetByColumnIdAsync(columnId);

        // Assert
        Assert.That(result, Has.All.Matches<TaskItem>(t => t.ColumnId == columnId));
    }

    [Test]
    public async Task GetByColumnIdAsync_ReturnsEmptyList_WhenColumnHasNoTasks()
    {
        // Arrange
        var emptyColumnId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByColumnIdAsync(emptyColumnId);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
