using Domain;
using Domain.Entities;
using Domain.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DomainTest;

[TestFixture]
public class UserRepositoryTest
{
    private AppDbContext _context = null!;
    private UserRepository _repository = null!;
    private User _user = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new UserRepository(_context);
        
        _user = new User
        {
            UserId = Guid.NewGuid(),
            GoogleId = "gid-setup",
            Username = "DefaultUser",
            Email = "default@example.com"
        };
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetByEmailAsync_ReturnsUser_WhenEmailExists()
    {
        // Arrange
        _context.Users.Add(_user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("default@example.com");

        // Assert
        Assert.That(result?.Email, Is.EqualTo("default@example.com"));
    }

    [Test]
    public async Task GetByEmailAsync_ReturnsNull_WhenEmailDoesNotExist()
    {
        // Act
        var result = await _repository.GetByEmailAsync("exemple@example.com");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUserTeamsAsync_ReturnsTeams_WhenUserIsInTeams()
    {
        // Arrange

        var team = new Team
        {
            TeamId = Guid.NewGuid(),
            TeamName = "My Team",
            Users = new[] { _user }
        };

        _context.Users.Add(_user);
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserTeamsAsync(_user.UserId);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetUserTeamsAsync_ReturnsEmpty_WhenUserNotInAnyTeam()
    {
        // Arrange
        _context.Users.Add(_user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUserTeamsAsync(_user.UserId);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
