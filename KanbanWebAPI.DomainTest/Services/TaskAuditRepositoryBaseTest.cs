using KanbanWebAPI.Domain;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.DomainTest.Services;

[TestFixture]
public class TaskAuditRepositoryBaseTest
{
    private AppDbContext _context = null!;
    private TaskAuditRepositoryBase _taskAuditRepositoryBase = null!;

    private const string Task1IdString = "00000000-0000-0000-0000-000000000100";
    private const string Task2IdString = "00000000-0000-0000-0000-000000000200";
    private const string NonExistentIdString = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";

    private static readonly List<TaskAudit> InitialAudits = TestDataLoader.LoadFromJson<TaskAudit>("TestTaskAudits.json");


    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _taskAuditRepositoryBase = new TaskAuditRepositoryBase(_context);

        _context.TaskAudit.AddRange(InitialAudits.Select(a => new TaskAudit
        {
            AuditId = a.AuditId,
            TaskItemId = a.TaskItemId,
            Action = a.Action,
            CreateAt = a.CreateAt,
            CreateByUserId = a.CreateByUserId
        }));

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private const string GetByTaskIdAsyncMethodName = nameof(ITaskAuditRepository.GetByTaskIdAsync);


    // GetByTaskIdAsync
    [TestCase(Task1IdString, 2, true, Description = "Returns audits for existing task, sorted descending",
        TestName = GetByTaskIdAsyncMethodName + " With Audits Sorted Desc")]
    [TestCase(Task2IdString, 1, false, Description = "Returns single audit for existing task",
        TestName = GetByTaskIdAsyncMethodName + " With Single Audit")]
    [TestCase(NonExistentIdString, 0, false,
        Description = "Returns empty list when task has no audits or doesn't exist",
        TestName = GetByTaskIdAsyncMethodName + " Empty List")]
    public async Task GetByTaskIdAsync(string taskIdString, int expectedCount, bool checkOrder)
    {
        // Arrange
        var taskId = Guid.Parse(taskIdString);

        // Act
        var result = await _taskAuditRepositoryBase.GetByTaskIdAsync(taskId);
        var resultList = result.ToList();

        // Assert
        if (expectedCount == 0)
        {
            Assert.That(resultList, Is.Empty, "The list should be empty for a non-existent task.");
        }
        else
        {
            Assert.That(resultList.Count(), Is.EqualTo(expectedCount));
            Assert.That(resultList.All(a => a.TaskItemId == taskId), Is.True);

            if (checkOrder)
            {
                Assert.That(resultList, Is.Ordered.Descending.By(nameof(TaskAudit.CreateAt)));
            }
        }
    }
}