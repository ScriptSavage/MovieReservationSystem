using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class MovieResepository : IMovieResepository
{
    private readonly DatabaseContext _context;

    public MovieResepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        return await _context.Movies.ToListAsync();
    }

    public async Task<Movie?> GetMovieAsync(long id)
    {
        return await _context.Movies
            .Where(e=>e.MovieId == id)
            .FirstOrDefaultAsync();
    }
}