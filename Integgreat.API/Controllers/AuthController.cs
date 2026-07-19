using Integgreat.API.Middleware;
using Integgreat.Application.DTOs.Auth;
using Integgreat.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Integgreat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }

    [HttpPost("register/client")]
    [SuperAdmin]
    public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterDto dto)
    {
        var result = await _authService.RegisterClientAsync(dto);
        return Ok(result);
    }

    [HttpPost("register/admin")]
    [SuperAdmin]
    public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDto dto)
    {
        var result = await _authService.RegisterAdminAsync(dto);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _authService.GetMeAsync(userId);
        return Ok(result);
    }
}