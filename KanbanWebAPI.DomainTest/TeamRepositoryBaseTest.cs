using System.Text.Json;

using KanbanWebAPI.Domain;
using KanbanWebAPI.Domain.Entities;
using KanbanWebAPI.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.DomainTest;

[TestFixture]
public class TeamRepositoryBaseTest
{
    private AppDbContext _context = null!;
    private TeamRepositoryBase _teamRepositoryBase = null!;

    private const string UserAliceIdString = "00000000-0000-0000-0000-000000000001";
    private const string UserBobIdString = "00000000-0000-0000-0000-000000000002";
    private const string NonExistentIdString = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF";
    private const string ExpectedTeamNameForAlice = "Development Team A";

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
        _teamRepositoryBase = new TeamRepositoryBase(_context);

        _context.Team.AddRange(InitialTeams);
        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private const string GetTeamMembersAsyncMethodName = nameof(ITeamRepository.GetTeamsByUserAsync);

    // GetTeamsByUserAsync
    [TestCase(UserAliceIdString, 1, ExpectedTeamNameForAlice, Description = "Returns teams for existing user Alice",
        TestName = GetTeamMembersAsyncMethodName + " User In Team A")]
    [TestCase(UserBobIdString, 1, ExpectedTeamNameForAlice, Description = "Returns teams for existing user Bob",
        TestName = GetTeamMembersAsyncMethodName + " User In Team B")]
    [TestCase(NonExistentIdString, 0, null, Description = "Returns empty list when user does not exist",
        TestName = GetTeamMembersAsyncMethodName + " Non Existent User")]
    public async Task GetTeamsByUserAsync(string userIdString, int expectedTeamCount,
        string? expectedTeamName)
    {
        // Arrange
        var userId = Guid.Parse(userIdString);

        // Act
        var result = await _teamRepositoryBase.GetTeamsByUserAsync(userId);
        var resultList = result.ToList();

        // Assert 
        if (expectedTeamCount == 0)
        {
            Assert.That(resultList, Is.Empty,
                "The list should be empty for a non-existent user or user not in a team.");
        }
        else
        {
            Assert.That(resultList.Count(), Is.EqualTo(expectedTeamCount),
                "The count of teams should match the expected count.");
        }
    }
}