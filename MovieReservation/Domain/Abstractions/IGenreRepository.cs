using Domain.Entities;

namespace Domain.Abstractions;

public interface IGenreRepository
{
    Task AddNewGenre(Genre? genre);
    Task<Genre?> GetGenre(long id);
    Task DeleteGenre(long id);
    Task<IEnumerable<Genre>> GetGenres();
    
    Task<List<Genre>> GetGenresByIdsAsync(IEnumerable<long> ids);
}