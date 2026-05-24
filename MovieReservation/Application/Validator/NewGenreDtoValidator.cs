using System.Data;
using Application.Dto.Genre;
using FluentValidation;

namespace Application.Validator;

public sealed class NewGenreDtoValidator : AbstractValidator<GenreDto.Request>
{
    public NewGenreDtoValidator()
    {
        RuleFor(e=>e.Name)
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long")
            .NotEmpty().WithMessage("Name is required");
    }
}