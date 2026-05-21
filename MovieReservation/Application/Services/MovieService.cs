using Application.Abstraction;
using Application.Dto.Movie;
using Domain.Abstractions;

namespace Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieResepository _movieRepository;

    public MovieService(IMovieResepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<List<MovieDto.Response>> GetAllMovies()
    {
        var movies = await _movieRepository.GetMoviesAsync();
        
        var result = movies.Select(e => 
            new MovieDto.Response(e.Title, e.OriginalTitle,e.Description,e.DurationInMinutes,
                e.ReleaseYear,e.MinimumAgeRecruitment)).ToList();
        
        return result;
    }

    public async Task<MovieDto.Response> GetMovie(long id)
    {
        var movie = await _movieRepository.GetMovieAsync(id);

        var result = new MovieDto.Response(movie.Title, movie.OriginalTitle,
            movie.Description,movie.DurationInMinutes,movie.ReleaseYear,movie.MinimumAgeRecruitment);
        
        return result;
    }
}