using Domain;
using Domain.Entities;
using Domain.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DomainTest;

[TestFixture]
public class TaskAuditRepositoryTest
{
    private AppDbContext _context = null!;
    private TaskAuditRepository _repository = null!;
    private Guid _task1;
    private Guid _task2;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new TaskAuditRepository(_context);

        _task1 = Guid.NewGuid();
        _task2 = Guid.NewGuid();

        _context.TaskAudits.AddRange(
            new TaskAudit
            {
                AuditId = Guid.NewGuid(),
                TaskId = _task1,
                ChangeDescription = "Created task",
                ChangedAt = DateTime.UtcNow.AddMinutes(-10),
                ChengedByUserId = Guid.NewGuid()
            },
            new TaskAudit
            {
                AuditId = Guid.NewGuid(),
                TaskId = _task1,
                ChangeDescription = "Updated description",
                ChangedAt = DateTime.UtcNow,
                ChengedByUserId = Guid.NewGuid()
            },
            new TaskAudit
            {
                AuditId = Guid.NewGuid(),
                TaskId = _task2,
                ChangeDescription = "Task moved to Done",
                ChangedAt = DateTime.UtcNow.AddMinutes(-5),
                ChengedByUserId = Guid.NewGuid()
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
    public async Task GetByTaskIdAsync_ReturnsAuditsForGivenTask_InDescendingOrder()
    {
        // Arrange
        var taskId = _task1;

        // Act
        var result = await _repository.GetByTaskIdAsync(taskId);
        var resultList = result.ToList();

        // Assert
        Assert.That(resultList, Is.Ordered.Descending.By(nameof(TaskAudit.ChangedAt))
            .And.All.Matches<TaskAudit>(a => a.TaskId == taskId));
    }

    [Test]
    public async Task GetByTaskIdAsync_ReturnsEmptyList_WhenTaskHasNoAudits()
    {
        // Arrange
        var taskWithoutAudits = Guid.NewGuid();

        // Act
        var result = await _repository.GetByTaskIdAsync(taskWithoutAudits);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
