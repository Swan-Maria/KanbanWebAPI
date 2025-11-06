using Domain;
using Domain.Entities;
using Domain.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DomainTest;

[TestFixture]
public class GenericRepositoryTest
{
    private AppDbContext _context = null!;
    private GenericRepository<Board> _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new GenericRepository<Board>(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task CreateAsync_AddsBoardToDatabase()
    {
        // Arrange
        var board = new Board { BoardId = Guid.NewGuid(), TeamId = Guid.NewGuid(), BoardName = "NewBoard" };

        // Act
        await _repository.CreateAsync(board);
        var savedBoard = await _context.Boards.FindAsync(board.BoardId);

        // Assert
        Assert.That(savedBoard, Is.Not.Null.And.Property(nameof(Board.BoardName)).EqualTo("NewBoard"));
    }

    [Test]
    public void CreateAsync_ThrowsException_WhenEntityIsNull()
    {
        // Arrange
        Board? nullBoard = null;

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _repository.CreateAsync(nullBoard!));
    }
    
    [Test]
    public async Task GetByIdAsync_ReturnsCorrectBoard()
    {
        // Arrange
        var board = new Board { BoardId = Guid.NewGuid(), TeamId = Guid.NewGuid(), BoardName = "Group Project" };
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(board.BoardId);

        // Assert
        Assert.That(result, Is.Not.Null.And.Property(nameof(Board.BoardId)).EqualTo(board.BoardId));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task GetAllAsync_ReturnsAllBoards()
    {
        // Arrange
        _context.Boards.AddRange(
            new Board { BoardId = Guid.NewGuid(), TeamId = Guid.NewGuid(), BoardName = "Board1" },
            new Board { BoardId = Guid.NewGuid(), TeamId = Guid.NewGuid(), BoardName = "Board2" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateAsync_ChangesBoardName()
    {
        // Arrange
        var board = new Board { BoardId = Guid.NewGuid(), TeamId = Guid.NewGuid(), BoardName = "OldName" };
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        // Act
        board.BoardName = "UpdatedName";
        await _repository.UpdateAsync(board);

        var updated = await _context.Boards.FindAsync(board.BoardId);

        // Assert
        Assert.That(updated!.BoardName, Is.EqualTo("UpdatedName"));
    }

    [Test]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoBoardsExist()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void UpdateAsync_ThrowsConcurrencyException_WhenEntityNotExists()
    {
        // Arrange
        var board = new Board { BoardId = Guid.NewGuid(), BoardName = "NewBoard" };

        // Act & Assert
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _repository.UpdateAsync(board));
    }


    [Test]
    public async Task DeleteAsync_RemovesBoardFromDatabase()
    {
        // Arrange
        var board = new Board { BoardId = Guid.NewGuid(), TeamId = Guid.NewGuid(), BoardName = "ToDelete" };
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(board.BoardId);
        var deleted = await _context.Boards.FindAsync(board.BoardId);

        // Assert
        Assert.That(deleted, Is.Null);
    }
    
    [Test]
    public async Task DeleteAsync_DoesNothing_WhenEntityNotFound()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        await _repository.DeleteAsync(nonExistingId);

        // Assert
        Assert.That(_context.Boards.Any(b => b.BoardId == nonExistingId), Is.False);
    }

}