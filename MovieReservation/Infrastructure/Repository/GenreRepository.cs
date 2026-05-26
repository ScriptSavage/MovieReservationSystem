using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class GenreRepository : IGenreRepository
{
    private readonly DatabaseContext _context;

    public GenreRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddNewGenre(Genre? genre)
    {
        await _context.Genres.AddAsync(genre) ;
        await _context.SaveChangesAsync();
    }

   

    public async Task<Genre?> GetGenre(long id) => await _context.Genres.FindAsync(id);

    public async Task<IEnumerable<Genre>> GetGenres() =>  await _context.Genres.ToListAsync();
    
    public async Task<List<Genre>> GetGenresByIdsAsync(IEnumerable<long> ids)
    {
        var genreIds = ids.ToList();

        return await _context.Genres
            .Where(g => genreIds.Contains(g.GenreId))
            .ToListAsync();
    }

    public async Task<bool> DoesGenreExist(long id)
    {
        return await _context.Genres.AnyAsync(g => g.GenreId == id);
    }

    public async Task DeleteGenre(long id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null) _context.Genres.Remove(genre);
    }
}