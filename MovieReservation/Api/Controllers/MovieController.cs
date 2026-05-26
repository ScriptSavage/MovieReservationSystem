using Application.Abstraction;
using Application.Dto.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMovies()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetMovie(long id)
    {
        var movie = await _movieService.GetMovie(id);
        return Ok(movie);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddMovie([FromBody] MovieDto.CreateMovieRequest createMovieRequest)
    {
        await _movieService.CreateMovie(createMovieRequest);
        return StatusCode(StatusCodes.Status201Created);
    }
    

    [HttpPost("{id:long}/genres/{genreId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddGenreToMovie(long id, long genreId)
    {
        await _movieService.AddGenreToMovie(id, genreId);
        return StatusCode(StatusCodes.Status201Created);
    }
    
    
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMovie(long id)
    {
        await _movieService.DeleteMovieAsync(id);
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpDelete("{id:long}/genres/{genreId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGenreFromMovie(long id, long genreId)
    {
        await _movieService.DeleteGenreFromMovieAsync(id, genreId);
        return Ok(new { Message = "Genre deleted from movie" });
    }


    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMovie(long id,[FromBody] MovieDto.UpdateMovieTitle request)
    {
        await _movieService.UpdateMovieAsync(id, request);
        return Ok(new { Message="Movie updated"});
    }
    
    
}