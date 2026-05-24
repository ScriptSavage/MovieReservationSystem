using Application.Dto.Movie;
using FluentValidation;

namespace Application.Validator;

public class NewMovieDtoValidator : AbstractValidator<MovieDto.CreateMovieRequest>
{
    public NewMovieDtoValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long");
    }
}