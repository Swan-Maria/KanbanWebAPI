using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using KanbanWebAPI.Domain;
using KanbanWebAPI.Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KanbanWebAPI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public AuthController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // ------------------- DTO -------------------

    public class AuthRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    // ------------------- REGISTER -------------------
    // POST: /api/Auth/register
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Email і пароль обов'язкові" });
        }

        var existing = _dbContext.User.FirstOrDefault(u => u.Email == request.Email);
        if (existing is not null)
        {
            return Conflict(new { message = "Користувач з таким email вже існує" });
        }

        CreatePasswordHash(request.Password, out var salt, out var hash);

        var username = request.Email.Contains('@')
            ? request.Email.Split('@')[0]
            : request.Email;

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = request.Email,
            Username = username,
            GoogleId = $"local-{Guid.NewGuid()}",
            PasswordSalt = Convert.ToBase64String(salt),
            PasswordHash = Convert.ToBase64String(hash)
        };

        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            message = "Користувача створено. Тепер можете логінитись.",
            userId = user.UserId
        });
    }

    // ------------------- LOGIN -------------------
    // POST: /api/Auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] AuthRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Email і пароль обов'язкові" });
        }

        var user = _dbContext.User.FirstOrDefault(u => u.Email == request.Email);
        if (user is null || string.IsNullOrEmpty(user.PasswordHash) || string.IsNullOrEmpty(user.PasswordSalt))
        {
            return Unauthorized(new { message = "Невірний логін або пароль" });
        }

        var saltBytes = Convert.FromBase64String(user.PasswordSalt);
        var hashBytes = Convert.FromBase64String(user.PasswordHash);

        if (!VerifyPassword(request.Password, saltBytes, hashBytes))
        {
            return Unauthorized(new { message = "Невірний логін або пароль" });
        }

        var token = CreateJwtToken(user);

        return Ok(new
        {
            token,
            expires = DateTime.UtcNow.AddHours(1)
        });
    }

    // ------------------- HELPERS -------------------

    private static void CreatePasswordHash(string password, out byte[] salt, out byte[] hash)
    {
        salt = RandomNumberGenerator.GetBytes(16);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        hash = pbkdf2.GetBytes(32);
    }

    private static bool VerifyPassword(string password, byte[] salt, byte[] storedHash)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var computedHash = pbkdf2.GetBytes(32);
        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }

    private static string CreateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new("userId", user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: JwtSettings.Issuer,
            audience: JwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}