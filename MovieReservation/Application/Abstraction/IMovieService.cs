using Application.Dto;
using Application.Dto.Movie;

namespace Application.Abstraction;

public interface IMovieService
{
    Task<List<MovieDto.Response>> GetAllMovies();

    Task<MovieDto.Response> GetMovie(long id);
    
    Task CreateMovie(MovieDto.CreateMovieRequest request);
    
    Task DeleteMovie(long id);
}