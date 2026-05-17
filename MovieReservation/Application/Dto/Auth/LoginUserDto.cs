namespace Application.Dto.Auth;

public static class LoginUserDto
{
    public record Request(string Email, string Password);
    
    public record Response(string AccessToken, string RefreshToken);
}