using System.Text.Json;

using AutoMapper;

using KanbanWebAPI.Application.DTOs.Columns;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Services;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Moq;


namespace KanbanWebAPI.ApplicationTest.Services
{
    [TestFixture]
    public class ColumnServiceTests
    {
        private Mock<IColumnRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private ColumnService _service;

        private static List<Column> LoadColumnsFromJson()
        {
            var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "TestColumns.json");
            if (!File.Exists(jsonFilePath))
                throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");
            var jsonString = File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<Column>>(jsonString) ?? new List<Column>();
        }

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<IColumnRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ColumnService(_mockRepo.Object, _mockMapper.Object);
        }


        private const string GetByBoardAsyncMethodName = nameof(IColumnService.GetByBoardAsync);
        private const string CreateAsyncMethodName = nameof(IColumnService.CreateAsync);
        private const string UpdateAsyncMethodName = nameof(IColumnService.UpdateAsync);
        private const string DeleteAsyncMethodName = nameof(IColumnService.DeleteAsync);

        [Test]
        [TestCase("11111111-1111-1111-1111-111111111111", 2, Description = "Board A has 2 columns", TestName = GetByBoardAsyncMethodName + " With existing board ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", 0, Description = "Non-existing board", TestName = GetByBoardAsyncMethodName + " With non-existing board ID")]
        public async Task GetByBoardAsync(string boardIdStr, int expectedCount)
        {
            var boardId = Guid.Parse(boardIdStr);
            var columns = LoadColumnsFromJson().Where(c => c.BoardId == boardId).ToList();

            _mockRepo.Setup(r => r.GetByBoardIdAsync(boardId)).ReturnsAsync(columns);
            _mockMapper.Setup(m => m.Map<IEnumerable<ColumnDto>>(It.IsAny<IEnumerable<Column>>()))
                .Returns((IEnumerable<Column> c) => c.Select(x =>
                    new ColumnDto { ColumnId = x.ColumnId, ColumnName = x.ColumnName, BoardId = x.BoardId }));

            var result = await _service.GetByBoardAsync(boardId);

            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task CreateAsync()
        {
            var createDto = new CreateColumnDto { ColumnName = "New Column", BoardId = Guid.NewGuid() };
            var entity = new Column { ColumnId = Guid.NewGuid(), ColumnName = createDto.ColumnName, BoardId = createDto.BoardId };

            _mockMapper.Setup(m => m.Map<Column>(It.IsAny<CreateColumnDto>())).Returns(entity);
            _mockRepo.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<ColumnDto>(entity)).Returns(new ColumnDto { ColumnId = entity.ColumnId, ColumnName = entity.ColumnName, BoardId = entity.BoardId });

            var result = await _service.CreateAsync(createDto);

            Assert.That(result.ColumnId, Is.EqualTo(entity.ColumnId));
            Assert.That(result.ColumnName, Is.EqualTo(entity.ColumnName));
        }


        [Test]
        [TestCase("44444444-4444-4444-4444-444444444444", true, Description = "Existing column to update", TestName = UpdateAsyncMethodName + " With existing column ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, Description = "Non-existing column to update", TestName = UpdateAsyncMethodName + " With non-existing column ID")]
        public async Task UpdateAsync(string columnIdStr, bool exists)
        {
            var columnId = Guid.Parse(columnIdStr);
            var dto = new UpdateColumnDto { ColumnName = "Updated Name" };
            var column = LoadColumnsFromJson().FirstOrDefault(c => c.ColumnId == columnId);

            _mockRepo.Setup(r => r.GetByIdAsync(columnId)).ReturnsAsync(column);
            if (column != null)
                _mockRepo.Setup(r => r.UpdateAsync(column)).Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map(dto, column ?? new Column()));
            _mockMapper.Setup(m => m.Map<ColumnDto>(It.IsAny<Column>()))
                .Returns((Column c) =>
                    new ColumnDto { ColumnId = c.ColumnId, ColumnName = c.ColumnName, BoardId = c.BoardId });

            var result = await _service.UpdateAsync(columnId, dto);

            Assert.That(result != null, Is.EqualTo(exists));
        }

        [Test]
        [TestCase("33333333-3333-3333-3333-333333333333", TestName = DeleteAsyncMethodName + " With existing column ID")]
        [TestCase("44444444-4444-4444-4444-444444444444", TestName = DeleteAsyncMethodName + " With non-existing column ID")]
        public async Task DeleteAsync(string columnIdStr)
        {
            var columnId = Guid.Parse(columnIdStr);

            _mockRepo.Setup(r => r.DeleteByIdAsync(columnId)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(columnId);

            _mockRepo.Verify(r => r.DeleteByIdAsync(columnId), Times.Once);
        }
    }
}