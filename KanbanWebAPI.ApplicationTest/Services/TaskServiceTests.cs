using System.Text.Json;

using AutoMapper;

using KanbanWebAPI.Application.DTOs.Tasks;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Services;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Moq;

namespace KanbanWebAPI.ApplicationTest.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskItemRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private TaskService _service;

        private static List<TaskItem> LoadTasksFromJson()
        {
            var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "TestTaskItems.json");
            if (!File.Exists(jsonFilePath))
                throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");
            var jsonString = File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(jsonString) ?? new List<TaskItem>();
        }

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<ITaskItemRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new TaskService(_mockRepo.Object, _mockMapper.Object);
        }


        private const string GetByColumnAsyncMethodName = nameof(ITaskService.GetByColumnAsync);
        private const string GetByIdAsyncMethodName = nameof(ITaskService.GetByIdAsync);
        private const string UpdateAsyncAsyncMethodName = nameof(ITaskService.GetByColumnAsync);
        private const string DeleteAsyncAsyncMethodName = nameof(ITaskService.CreateAsync);


        [Test]
        [TestCase("33333333-3333-3333-3333-333333333333", 3, Description = "Column A has 3 tasks", TestName = GetByColumnAsyncMethodName + " With existing column ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", 0, Description = "Non-existing task", TestName = GetByColumnAsyncMethodName + " With non-existing column ID")]
        public async Task GetByColumnAsync(string columnIdStr, int expectedCount)
        {
            var columnId = Guid.Parse(columnIdStr);
            var tasks = LoadTasksFromJson().Where(t => t.ColumnId == columnId).ToList();

            _mockRepo.Setup(r => r.GetByColumnIdAsync(columnId)).ReturnsAsync(tasks);
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskDto>>(It.IsAny<IEnumerable<TaskItem>>()))
                .Returns((IEnumerable<TaskItem> t) => t.Select(x => new TaskDto
                {
                    TaskId = x.TaskId,
                    Title = x.Title,
                    ColumnId = x.ColumnId
                }));

            var result = await _service.GetByColumnAsync(columnId);

            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }


        [Test]
        [TestCase("55555555-5555-5555-5555-555555555555", true, Description = "Existing task", TestName = GetByColumnAsyncMethodName + " With existing task ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, Description = "Non-existing task", TestName = GetByColumnAsyncMethodName + " With non-existing task ID")]
        public async Task GetByIdAsync(string taskIdStr, bool exists)
        {
            var taskId = Guid.Parse(taskIdStr);
            var task = LoadTasksFromJson().FirstOrDefault(t => t.TaskId == taskId);

            _mockRepo.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
            _mockMapper.Setup(m => m.Map<TaskDto>(It.IsAny<TaskItem>()))
                .Returns((TaskItem t) => new TaskDto { TaskId = t.TaskId, Title = t.Title, ColumnId = t.ColumnId });

            var result = await _service.GetByIdAsync(taskId);

            Assert.That(result != null, Is.EqualTo(exists));
        }

        [Test]
        public async Task CreateAsync()
        {
            var createDto = new CreateTaskDto { Title = "New Task", ColumnId = Guid.NewGuid() };
            var entity = new TaskItem { TaskId = Guid.NewGuid(), Title = createDto.Title, ColumnId = createDto.ColumnId };

            _mockMapper.Setup(m => m.Map<TaskItem>(It.IsAny<CreateTaskDto>())).Returns(entity);
            _mockRepo.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<TaskDto>(entity))
                .Returns(new TaskDto { TaskId = entity.TaskId, Title = entity.Title, ColumnId = entity.ColumnId });

            var result = await _service.CreateAsync(createDto);

            Assert.That(result.TaskId, Is.EqualTo(entity.TaskId));
            Assert.That(result.Title, Is.EqualTo(entity.Title));
        }

        [Test]
        [TestCase("55555555-5555-5555-5555-555555555555", true, Description = "Existing task to update", TestName = UpdateAsyncAsyncMethodName + " With existing task ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, Description = "Non-existing task to update", TestName = UpdateAsyncAsyncMethodName + " With non-existing task ID")]
        public async Task UpdateAsync(string taskIdStr, bool exists)
        {
            var taskId = Guid.Parse(taskIdStr);
            var dto = new UpdateTaskDto { Title = "Updated Title" };
            var task = LoadTasksFromJson().FirstOrDefault(t => t.TaskId == taskId);

            _mockRepo.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
            if (task != null)
                _mockRepo.Setup(r => r.UpdateAsync(task)).Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map(dto, task ?? new TaskItem()));
            _mockMapper.Setup(m => m.Map<TaskDto>(It.IsAny<TaskItem>()))
                .Returns((TaskItem t) => new TaskDto { TaskId = t.TaskId, Title = t.Title, ColumnId = t.ColumnId });

            var result = await _service.UpdateAsync(taskId, dto);

            Assert.That(result != null, Is.EqualTo(exists));
        }

        [Test]
        [TestCase("55555555-5555-5555-5555-555555555555", TestName = DeleteAsyncAsyncMethodName + " With existing task ID")]
        [TestCase("66666666-6666-6666-6666-666666666666", TestName = DeleteAsyncAsyncMethodName + " With non-existing task ID")]
        public async Task DeleteAsync(string taskIdStr)
        {
            var taskId = Guid.Parse(taskIdStr);

            _mockRepo.Setup(r => r.DeleteByIdAsync(taskId)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(taskId);

            _mockRepo.Verify(r => r.DeleteByIdAsync(taskId), Times.Once);
        }
    }
}