using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanWebAPI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProfile()
    {
        if (User?.Identity is null || !User.Identity.IsAuthenticated)
            return Unauthorized();

        var userId = User.FindFirstValue("userId");
        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            UserId = userId,
            Email = email,
            Name = name,
            Role = role
        });
    }
}