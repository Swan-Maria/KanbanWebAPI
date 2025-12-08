using System.Text.Json;

using AutoMapper;

using KanbanWebAPI.Application.DTOs.Teams;
using KanbanWebAPI.Application.Interfaces.Services;
using KanbanWebAPI.Application.Services;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Moq;

namespace KanbanWebAPI.ApplicationTest.Services
{
    [TestFixture]
    public class TeamServiceTests
    {
        private Mock<ITeamRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private TeamService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<ITeamRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new TeamService(_mockRepo.Object, _mockMapper.Object);
        }

        private const string GetUserTeamsAsyncMethodName = nameof(ITeamService.GetUserTeamsAsync);
        private const string UpdateAsyncMethodName = nameof(ITeamService.UpdateAsync);
        private const string DeleteAsyncMethodName = nameof(ITeamService.DeleteAsync);

        [Test]
        [TestCase("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 2, Description = "User A is in 2 teams", TestName = GetUserTeamsAsyncMethodName + " With existing user ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", 0, Description = "Non-existing user", TestName = GetUserTeamsAsyncMethodName + " With non-existing user ID")]
        public async Task GetUserTeamsAsync(string userIdStr, int expectedCount)
        {
            var userId = Guid.Parse(userIdStr);
            var teams = TestDataLoader.LoadFromJson<Team>("TestTeams.json").Where(t => t.Users.Any(u => u.UserId == userId)).ToList();

            _mockRepo.Setup(r => r.GetTeamsByUserAsync(userId)).ReturnsAsync(teams);
            _mockMapper.Setup(m => m.Map<IEnumerable<TeamDto>>(It.IsAny<IEnumerable<Team>>()))
                .Returns((IEnumerable<Team> t) => t.Select(x => new TeamDto { TeamId = x.TeamId, TeamName = x.TeamName }));

            var result = await _service.GetUserTeamsAsync(userId);

            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task CreateAsync()
        {
            var createDto = new CreateTeamDto { TeamName = "New Team" };
            var entity = new Team { TeamId = Guid.NewGuid(), TeamName = createDto.TeamName };

            _mockMapper.Setup(m => m.Map<Team>(It.IsAny<CreateTeamDto>())).Returns(entity);
            _mockRepo.Setup(r => r.CreateAsync(entity)).ReturnsAsync(entity);
            _mockMapper.Setup(m => m.Map<TeamDto>(entity))
                .Returns(new TeamDto { TeamId = entity.TeamId, TeamName = entity.TeamName });

            var result = await _service.CreateAsync(createDto);

            Assert.That(result.TeamId, Is.EqualTo(entity.TeamId));
            Assert.That(result.TeamName, Is.EqualTo(entity.TeamName));
        }

        [Test]
        [TestCase("11111111-1111-1111-1111-111111111111", true, Description = "Existing team to update", TestName = UpdateAsyncMethodName + " With existing team ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", false, Description = "Non-existing team to update", TestName = UpdateAsyncMethodName + " With non-existing team ID")]
        public async Task UpdateAsync(string teamIdStr, bool exists)
        {
            var teamId = Guid.Parse(teamIdStr);
            var dto = new UpdateTeamDto { TeamName = "Updated Name" };
            var team = TestDataLoader.LoadFromJson<Team>("TestTeams.json").FirstOrDefault(t => t.TeamId == teamId);

            _mockRepo.Setup(r => r.GetByIdAsync(teamId)).ReturnsAsync(team);
            if (team != null)
                _mockRepo.Setup(r => r.UpdateAsync(team)).Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map(dto, team ?? new Team()));
            _mockMapper.Setup(m => m.Map<TeamDto>(It.IsAny<Team>()))
                .Returns((Team t) => new TeamDto { TeamId = t.TeamId, TeamName = t.TeamName });

            var result = await _service.UpdateAsync(teamId, dto);

            Assert.That(result != null, Is.EqualTo(exists));
        }

        [Test]
        [TestCase("11111111-1111-1111-1111-111111111111", TestName = DeleteAsyncMethodName + " With existing team ID")]
        [TestCase("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", TestName = DeleteAsyncMethodName + " With non-existing team ID")]
        public async Task DeleteAsync(string teamIdStr)
        {
            var teamId = Guid.Parse(teamIdStr);

            _mockRepo.Setup(r => r.DeleteByIdAsync(teamId)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(teamId);

            _mockRepo.Verify(r => r.DeleteByIdAsync(teamId), Times.Once);
        }
    }
}