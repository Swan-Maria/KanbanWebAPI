using Domain;
using Domain.Entities;
using Domain.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DomainTest;

[TestFixture]
public class TeamRepositoryTest
{
    private AppDbContext _context = null!;
    private TeamRepository _repository = null!;
    private User _user = null!;
    private Team _team = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new TeamRepository(_context);
        
        
        _user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "John",
            Email = "john@example.com",
            GoogleId = "google-123"
        };
        _team = new Team
        {
            TeamId = Guid.NewGuid(),
            TeamName = "Team1",
            Users = new[] { _user }
        };
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetTeamUsersAsync_ReturnsOneUser_WhenTeamHasUser()
    {
        // Arrange
        _context.Users.Add(_user);
        _context.Teams.Add(_team);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTeamUsersAsync(_team.TeamId);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetTeamUsersAsync_ReturnsCorrectUserName_WhenTeamHasUser()
    {
        // Arrange
        _context.Users.Add(_user);
        _context.Teams.Add(_team);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTeamUsersAsync(_team.TeamId);

        // Assert
        Assert.That(result.First().Username, Is.EqualTo("John"));
    }

    [Test]
    public async Task GetTeamUsersAsync_ReturnsEmpty_WhenTeamHasNoUsers()
    {
        // Arrange
        var emptyTeam = new Team
        {
            TeamId = Guid.NewGuid(),
            TeamName = "EmptyTeam",
            Users = new List<User>()
        };
        _context.Teams.Add(emptyTeam);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTeamUsersAsync(emptyTeam.TeamId);

        // Assert
        Assert.That(result, Is.Empty, "Expected empty user list for a team without users");
    }

    [Test]
    public async Task GetTeamUsersAsync_ReturnsEmpty_WhenTeamDoesNotExist()
    {
        // Arrange
        var nonExistingTeamId = Guid.NewGuid();

        // Act
        var result = await _repository.GetTeamUsersAsync(nonExistingTeamId);

        // Assert
        Assert.That(result, Is.Empty);
    }
}