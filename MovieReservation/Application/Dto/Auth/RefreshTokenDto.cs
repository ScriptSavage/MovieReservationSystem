namespace Application.Dto.Auth;

public static class RefreshTokenDto
{
    public record Request(string RefreshToken);
}