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
}