namespace Application.Dto.Auth;

public static class RegisterUserDto
{
    public record RegisterUserRequest(string FirstName , string LastName ,string Email, string PhoneNumber, 
        string Password, string ConfirmPassword);
    
}