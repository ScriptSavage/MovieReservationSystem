namespace Application.Dto.Auth;

public record ChangePasswordDto(string OldPassword, 
    string NewPassword, string ConfirmNewPassword);