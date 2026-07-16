using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftlarePMS.Application.DTOs.Auth;
using SoftlarePMS.Application.Features.Auth.Commands.Login;
using SoftlarePMS.Application.Features.Auth.Commands.RefreshToken;

namespace SoftlarePMS.WebApi.Controllers;

[AllowAnonymous]
public sealed class AuthController : ApiControllerBase
{
    /// <summary>Authenticate with username and password. Returns JWT access + refresh tokens.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct);
        return Ok(result);
    }

    /// <summary>Exchange an expired access token and a valid refresh token for a new token pair.</summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(JwtTokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await Sender.Send(command, ct);
        return Ok(result);
    }
}
