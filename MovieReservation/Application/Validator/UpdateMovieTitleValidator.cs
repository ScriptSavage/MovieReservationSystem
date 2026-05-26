using Application.Dto.Movie;
using FluentValidation;

namespace Application.Validator;

public class UpdateMovieTitleValidator : AbstractValidator<MovieDto.UpdateMovieTitle>
{
    public UpdateMovieTitleValidator()
    {
        RuleFor(request => request.NewTitle)
            .NotNull()
            .NotEmpty()
            .MaximumLength(250).WithMessage("Title must not exceed 250 characters.")
            .MinimumLength(2).WithMessage("Title must not exceed 2 characters.");
    }
}