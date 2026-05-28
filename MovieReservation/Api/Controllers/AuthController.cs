using System.Security.Claims;
using Application.Abstraction;
using Application.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto.RegisterUserRequest request)
    {
        var result = await _authService.RegisterUser(request);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("User has been registered");
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserDto.Response>> Login([FromBody] LoginUserDto.Request request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<LoginUserDto.Response>> RefreshToken([FromBody] RefreshTokenDto.Request request)
    {
        var response = await _authService.RefreshTokenAsync(request);

        return Ok(response);
    }
}