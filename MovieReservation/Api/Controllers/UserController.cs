using System.Security.Claims;
using Application.Abstraction;
using Application.Dto.Auth;
using Application.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/me")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPatch("change-password")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        await _userService.ChangePasswordAsync(userId, dto);

        return Ok("Password has been changed");
    }


    [HttpPatch("change-email")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        await _userService.ChangeEmailAsync(userId,dto);
        return Ok("Email has been changed");
    }

    [HttpDelete]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }
      
        await _userService.DeleteAccountAsync(userId);
        
        return Ok("Account has been deleted");
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        { 
            return Unauthorized();
        }
        var userDetails = await _userService.GetUserDetailsAsync(userId);
        return Ok(userDetails);
    }


    [HttpGet("reservations")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetUserReservations()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }
        var userReservations = await _userService.GetAllReservationsAsync(userId);
        return Ok(userReservations);
    }
}