using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class MovieRepository : IMovieResepository
{
    private readonly DatabaseContext _context;

    public MovieRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        return await _context.Movies
            .Include(e=>e.Genres)
            .ToListAsync();
    }

    public async Task<Movie?> GetMovieAsync(long id)
    {
        return await _context.Movies
            .Include(e=>e.Genres)
            .Where(e=>e.MovieId == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddNewMovie(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    public Task<bool> DoesMovieExist(long id)
    {
       return _context.Movies.AnyAsync(e => e.MovieId == id);
    }
}