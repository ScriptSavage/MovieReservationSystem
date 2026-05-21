using Application.Abstraction;
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
}