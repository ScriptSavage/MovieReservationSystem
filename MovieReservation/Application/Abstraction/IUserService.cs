using Application.Dto.Auth;

namespace Application.Abstraction;

public interface IUserService
{
    Task ChangePasswordAsync(string userId ,ChangePasswordDto dto);
}