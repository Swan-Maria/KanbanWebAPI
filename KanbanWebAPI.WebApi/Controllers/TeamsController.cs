using System.Security.Claims;

using KanbanWebAPI.Application.DTOs.Teams;
using KanbanWebAPI.Application.Interfaces.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanWebAPI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetMyTeams()
    {
        var userIdStr = User.FindFirstValue("userId");
        if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
            return Unauthorized(new { message = "UserId claim missing or invalid" });

        var teams = await _teamService.GetUserTeamsAsync(userId);
        return Ok(teams);
    }

    [HttpPost]
    public async Task<ActionResult<TeamDto>> Create([FromBody] CreateTeamDto dto)
    {
        var team = await _teamService.CreateAsync(dto);
        return Ok(team);
    }

    [HttpPut("{teamId:guid}")]
    public async Task<ActionResult<TeamDto>> Update(Guid teamId, [FromBody] UpdateTeamDto dto)
    {
        var updated = await _teamService.UpdateAsync(teamId, dto);
        if (updated is null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{teamId:guid}")]
    public async Task<IActionResult> Delete(Guid teamId)
    {
        await _teamService.DeleteAsync(teamId);
        return NoContent();
    }
}