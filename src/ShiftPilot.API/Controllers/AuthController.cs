using Microsoft.AspNetCore.Mvc;
using ShiftPilot.API.Services;
using ShiftPilot.API.DTOs;

namespace ShiftPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, token, message) = await _authService.RegisterAsync(
            request.Email, request.FirstName, request.LastName, request.Password);

        if (!success)
            return BadRequest(new { message });

        return Ok(new { token, message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, token, message) = await _authService.LoginAsync(request.Email, request.Password);

        if (!success)
            return Unauthorized(new { message });

        return Ok(new { token, message });
    }
}
