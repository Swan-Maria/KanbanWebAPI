using System.Text.Json;

using KanbanWebAPI.Domain;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.DomainTest;

[TestFixture]
public class UserRepositoryBaseTest
{
    private AppDbContext _context = null!;
    private UserRepositoryBase _userRepositoryBase = null!;

    private const string UserAliceEmail = "alice@example.com";
    private const string NonExistentEmail = "nonexistent@example.com";
    private const string TeamAIdString = "00000000-0000-0000-0000-000000000100";
    private const string TeamBIdString = "00000000-0000-0000-0000-000000000200";
    private const string NonExistentTeamIdString = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";

    private static readonly List<Team> InitialTeams = LoadTeamsFromJson();

    private static List<Team> LoadTeamsFromJson()
    {
        var jsonFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestTeamMembers.json");
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"JSON data file {jsonFilePath} not found");
        }

        var jsonString = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<Team>>(jsonString) ?? new List<Team>();
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _userRepositoryBase = new UserRepositoryBase(_context);

        _context.Team.AddRange(InitialTeams);
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private const string GetByEmailAsyncMethodName = nameof(IUserRepository.GetByEmailAsync);
    private const string GetTeamMembersAsyncMethodName = nameof(IUserRepository.GetUsersByTeamAsync);


    // GetByEmailAsync
    [TestCase(UserAliceEmail, true, Description = "Get existing user by email",
        TestName = GetByEmailAsyncMethodName + " With Existing Email")]
    [TestCase(NonExistentEmail, false, Description = "Get null for non-existing email",
        TestName = GetByEmailAsyncMethodName + " With Non Existing Email")]
    public async Task GetByEmailAsync(string email, bool shouldExist)
    {
        // Arrange

        // Act
        var result = await _userRepositoryBase.GetByEmailAsync(email);

        // Assert
        if (shouldExist)
        {
            Assert.That(result, Is.Not.Null, "Result should not be null for an existing email.");
            Assert.That(result.Email, Is.EqualTo(email), "The retrieved user's email must match the input email.");
        }
        else
        {
            Assert.That(result, Is.Null, "Result should be null for a non-existent email.");
        }
    }

    // GetUsersByTeamAsync
    [TestCase(TeamAIdString, 2, "Alice", Description = "Returns members for existing team A",
        TestName = GetTeamMembersAsyncMethodName + " Team A Users")]
    [TestCase(TeamBIdString, 0, null, Description = "Returns empty list when team has no users",
        TestName = GetTeamMembersAsyncMethodName + " Team B No Users")]
    [TestCase(NonExistentTeamIdString, 0, null, Description = "Returns empty list when team does not exist",
        TestName = GetTeamMembersAsyncMethodName + " Non Existent Team")]
    public async Task GetUsersByTeamAsync(string teamIdString, int expectedCount, string? expectedUserName)
    {
        // Arrange
        var teamId = Guid.Parse(teamIdString);

        // Act
        var result = await _userRepositoryBase.GetUsersByTeamAsync(teamId);
        var resultList = result.ToList();

        // Assert
        if (expectedCount == 0)
        {
            Assert.That(resultList, Is.Empty,
                "The list should be empty for a team without users or non-existent team.");
        }
        else
        {
            Assert.That(resultList.Count(), Is.EqualTo(expectedCount),
                "The count of users should match the expected count.");
        }
    }
}