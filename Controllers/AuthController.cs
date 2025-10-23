using AuthService.DTOs;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService.Interfaces.IAuthService _authService;

    public AuthController(AuthService.Interfaces.IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthService.DTOs.LoginRequestDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthService.DTOs.RegisterRequestDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        return Ok(result);
    }
}
