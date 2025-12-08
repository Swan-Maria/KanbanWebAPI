using System.Text.Json;

using AutoMapper;

using KanbanWebAPI.Application.DTOs.Users;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Services;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Moq;

namespace KanbanWebAPI.ApplicationTest.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new UserService(_mockRepo.Object, _mockMapper.Object);
        }

        private const string GetByIdAsyncMethodName = nameof(IUserService.GetByIdAsync);

        [Test]
        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", true, Description = "Existing user", TestName = GetByIdAsyncMethodName + " With existing user ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, Description = "Non-existing user", TestName = GetByIdAsyncMethodName + " With non-existing user ID")]
        public async Task GetByIdAsync(string userIdStr, bool exists)
        {
            var userId = Guid.Parse(userIdStr);
            var user = TestDataLoader.LoadFromJson<User>("TestUsers.json").FirstOrDefault(u => u.UserId == userId);

            _mockRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns((User u) => new UserDto { UserId = u.UserId, Username = u.Username });

            var result = await _service.GetByIdAsync(userId);

            Assert.That(result != null, Is.EqualTo(exists));
        }

        [Test]
        public async Task GetAllAsync()
        {
            var users = TestDataLoader.LoadFromJson<User>("TestUsers.json");

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                .Returns((IEnumerable<User> u) =>
                    u.Select(x => new UserDto { UserId = x.UserId, Username = x.Username }));

            var result = await _service.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(users.Count));
        }
    }
}