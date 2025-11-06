using Domain;
using Domain.Entities;
using Domain.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DomainTest;

[TestFixture]
public class BoardRepositoryTest
{
    private AppDbContext _context = null!;
    private BoardRepository _repository = null!;
    private readonly Guid _team1 = Guid.NewGuid();
    private readonly Guid _team2 = Guid.NewGuid();


    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("BoardRepositoryTest")
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _context.Boards.RemoveRange(_context.Boards);
        _context.SaveChanges();

        _repository = new BoardRepository(_context);

        _context.Boards.AddRange(
            new Board { BoardId = Guid.NewGuid(), TeamId = _team1, BoardName = "Project1" },
            new Board { BoardId = Guid.NewGuid(), TeamId = _team1, BoardName = "Project2" },
            new Board { BoardId = Guid.NewGuid(), TeamId = _team2, BoardName = "Project3" }
        );

        _context.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
    
    [Test]
    public async Task GetByTeamIdAsync_ReturnsBoardsForSpecificTeam()
    {
        // Arrange
        var teamId = _context.Boards.First().TeamId;

        // Act
        var result = await _repository.GetByTeamIdAsync(teamId);

        // Assert
        Assert.That(result, Has.All.Matches<Board>(b => b.TeamId == teamId));
    }

    [Test]
    public async Task GetByTeamIdAsync_ReturnsEmptyList_WhenTeamHasNoBoards()
    {
        // Arrange
        var teamWithoutBoards = Guid.NewGuid();

        // Act
        var result = await _repository.GetByTeamIdAsync(teamWithoutBoards);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByNameAsync_ReturnsCorrectBoard()
    {
        // Arrange
        var existingBoard = _context.Boards.First();

        // Act
        var result = await _repository.GetByNameAsync(existingBoard.TeamId, existingBoard.BoardName);

        // Assert
        Assert.That(result,
            Is.Not.Null.And.Matches<Board>(b =>
                b.BoardName == existingBoard.BoardName && b.TeamId == existingBoard.TeamId));
    }

    [Test]
    public async Task GetByNameAsync_ReturnsNull_WhenBoardDoesNotExist()
    {
        // Arrange
        var nonExistingTeamId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByNameAsync(nonExistingTeamId, "NonExistentBoard");

        // Assert
        Assert.That(result, Is.Null);
    }
}