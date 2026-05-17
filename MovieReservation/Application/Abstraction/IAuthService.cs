using Application.Dto.Auth;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstraction;

public interface IAuthService
{
    Task<IdentityResult> RegisterUser(RegisterUserDto.RegisterUserRequest dto);
    
    Task<LoginUserDto.Response> LoginAsync(LoginUserDto.Request dto);
    
    Task<LoginUserDto.Response> RefreshTokenAsync(RefreshTokenDto.Request dto);
}