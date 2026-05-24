using Application.Dto.Genre;
using Domain.Entities;

namespace Application.Abstraction;

public interface IGenreService 
{
    Task AddGenre(GenreDto.Request genre);
    Task <GenreDto.Response> GetGenre(long id);
    Task<IEnumerable<GenreDto.Response>> GetAllGenres();
    Task RemoveGenre(long genreId);
}