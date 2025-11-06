using Domain;
using Domain.Entities;
using Domain.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DomainTest;

[TestFixture]
public class ColumnRepositoryTest
{
    private AppDbContext _context = null!;
    private ColumnRepository _repository = null!;
    private Guid _board1;
    private Guid _board2;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new ColumnRepository(_context);

        _board1 = Guid.NewGuid();
        _board2 = Guid.NewGuid();

        _context.Columns.AddRange(
            new Column
            {
                ColumnId = Guid.NewGuid(),
                BoardId = _board1,
                ColumnName = "ToDo",
                Tasks = new List<TaskItem>
                {
                    new TaskItem { TaskId = Guid.NewGuid(), Title = "Task 1" },
                    new TaskItem { TaskId = Guid.NewGuid(), Title = "Task 2" }
                }
            },
            new Column
            {
                ColumnId = Guid.NewGuid(),
                BoardId = _board1,
                ColumnName = "In Progress",
                Tasks = new List<TaskItem>
                {
                    new TaskItem { TaskId = Guid.NewGuid(), Title = "Task 3" }
                }
            },
            new Column
            {
                ColumnId = Guid.NewGuid(),
                BoardId = _board2,
                ColumnName = "Done"
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
    public async Task GetByBoardIdAsync_ReturnsColumnsWithTasks_ForGivenBoard()
    {
        // Arrange
        var boardId = _board1;

        // Act
        var result = await _repository.GetByBoardIdAsync(boardId);

        // Assert (один Assert по логике)
        Assert.That(result.All(c => c.BoardId == boardId));
    }

    [Test]
    public async Task GetByBoardIdAsync_ReturnsEmptyList_WhenBoardHasNoColumns()
    {
        // Arrange
        var boardWithoutColumns = Guid.NewGuid();

        // Act
        var result = await _repository.GetByBoardIdAsync(boardWithoutColumns);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
