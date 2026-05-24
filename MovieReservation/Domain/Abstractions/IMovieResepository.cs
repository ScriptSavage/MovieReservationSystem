using Domain.Entities;

namespace Domain.Abstractions;

public interface IMovieResepository
{
    Task<List<Movie>> GetMoviesAsync();
    Task<Movie?> GetMovieAsync(long id);

    Task AddNewMovie(Movie movie);
    
    Task<bool> DoesMovieExist(string originalTitle);
    
    Task DeleteMovie(long id);


}