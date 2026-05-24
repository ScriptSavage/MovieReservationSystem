using Application.Abstraction;
using Application.Dto.Genre;
using Application.Dto.Movie;
using Domain.Abstractions;
using Domain.Entities;
using FluentValidation;

namespace Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieResepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IValidator<MovieDto.CreateMovieRequest> _movieDtoValidator;

    public MovieService(IMovieResepository movieRepository, 
        IGenreRepository genreRepository,
        IValidator<MovieDto.CreateMovieRequest> movieDtoValidator)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _movieDtoValidator = movieDtoValidator;
    }

    public async Task<List<MovieDto.Response>> GetAllMovies()
    {
        var movies = await _movieRepository.GetMoviesAsync();

        var result = movies.Select(e =>
            new MovieDto.Response(e.Title, 
                e.OriginalTitle, 
                e.Description, 
                e.DurationInMinutes,
                e.ReleaseYear, 
                e.MinimumAgeRecruitment,
                e.Genres.Select(x=> new GenreDto.Response(x.GenreName)).ToList()))
            .ToList();
        
        return result;
    }

    public async Task<MovieDto.Response> GetMovie(long id)
    {
        var movie = await _movieRepository.GetMovieAsync(id);

        if (movie is null)
        {
            throw new ArgumentException("Movie not found.");
        }

        var result = new MovieDto.Response(
            movie.Title,
            movie.OriginalTitle,
            movie.Description,
            movie.DurationInMinutes,
            movie.ReleaseYear,
            movie.MinimumAgeRecruitment,
            movie.Genres
                .Select(x => new GenreDto.Response(x.GenreName))
                .ToList()
        );

        return result;
    }

    public async Task CreateMovie(MovieDto.CreateMovieRequest request)
    {
        await _movieDtoValidator.ValidateAsync(request);
        
        var genreIds = request.GenresId
            .Distinct()
            .ToList();

        if (genreIds.Count == 0)
        {
            throw new ArgumentException("Movie must have at least one genre.");
        }

        var genres = await _genreRepository.GetGenresByIdsAsync(genreIds);

        if (genres.Count != genreIds.Count)
        {
            throw new ArgumentException("One or more genres do not exist.");
        }

        var newMovie = new Movie
        {
            Title = request.Title,
            OriginalTitle = request.OriginalTitle,
            Description = request.Description,
            DurationInMinutes = request.DurationInMinutes,
            ReleaseYear = request.ReleaseYear,
            MinimumAgeRecruitment = request.MinimumAgeRecruitment,
            Genres = genres
        };

        await _movieRepository.AddNewMovie(newMovie);
    }
    
}