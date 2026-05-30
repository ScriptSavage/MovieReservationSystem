namespace Application.Dto.User;

public record UserDetailsDto(
    string Email, 
    string FirstName, 
    string LastName, 
    string PhoneNumber);