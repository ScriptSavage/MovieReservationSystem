using Application.Dto;
using Application.Dto.Movie;

namespace Application.Abstraction;

public interface IMovieService
{
    Task<List<MovieDto.Response>> GetAllMovies();

    Task<MovieDto.Response> GetMovie(long id);
    
    Task CreateMovie(MovieDto.CreateMovieRequest request);
    
    Task DeleteMovieAsync(long id);
    
    Task UpdateMovieAsync(long id, MovieDto.UpdateMovieTitle request);
    
    Task AddGenreToMovie(long id, long genreId);
    
    Task DeleteGenreFromMovieAsync(long id, long genreId);
}