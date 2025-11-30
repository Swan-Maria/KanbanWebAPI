using System.Text.Json;

using AutoMapper;

using KanbanWebAPI.Application.DTOs.TaskAudits;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Services;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Moq;


namespace KanbanWebAPI.ApplicationTest.Services;
[TestFixture]
public class TaskAuditServiceTests
{
    private Mock<ITaskAuditRepository> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private TaskAuditService _service;

    private static List<TaskAudit> LoadTaskAuditsFromJson()
    {
        var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "TestTaskAudits.json");
        if (!File.Exists(jsonFilePath)) throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");
        var jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<TaskAudit>>(jsonString) ?? new List<TaskAudit>();
    }

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<ITaskAuditRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new TaskAuditService(_mockRepo.Object, _mockMapper.Object);
    }


    private const string GetByTaskAsyncMethodName = nameof(ITaskAuditService.GetByTaskAsync);

    [Test]
    [TestCase("11111111-1111-1111-1111-111111111111", 2, TestName = GetByTaskAsyncMethodName + " With existing task ID")]
    [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", 0, TestName = GetByTaskAsyncMethodName + " With non-existing task ID")]
    public async Task GetByTaskAsync(string taskIdStr, int expectedCount)
    {
        var taskId = Guid.Parse(taskIdStr);
        var audits = LoadTaskAuditsFromJson().Where(a => a.TaskItemId == taskId).ToList();
        _mockRepo.Setup(r => r.GetByTaskIdAsync(taskId)).ReturnsAsync(audits);
        _mockMapper.Setup(m => m.Map<IEnumerable<TaskAuditDto>>(It.IsAny<IEnumerable<TaskAudit>>()))
            .Returns((IEnumerable<TaskAudit> t) =>
                t.Select(x => new TaskAuditDto { AuditId = x.AuditId, TaskItemId = x.TaskItemId }));
        var result = await _service.GetByTaskAsync(taskId);
        Assert.That(result.Count(), Is.EqualTo(expectedCount));
    }
}