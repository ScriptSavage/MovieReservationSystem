using Application.Dto.Auth;
using FluentValidation;

namespace Application.Validator;

public sealed class UserRegistrationDtoValidator : AbstractValidator<RegisterUserDto.RegisterUserRequest>
{
    public UserRegistrationDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters");


        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required");
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Password must have at least 6 characters")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters");
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.Password).WithMessage("Passwords don't match");
        
    }
}