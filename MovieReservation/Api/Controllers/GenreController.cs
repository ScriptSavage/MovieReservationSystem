using Application.Abstraction;
using Application.Dto.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/genres")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetGenre(long id)
    {
        var genre =  await _genreService.GetGenre(id);
        return Ok(genre);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenres()
    {
        var gereList =  await _genreService.GetAllGenres();
        return Ok(gereList);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddGenre([FromBody] GenreDto.Request genre)
    {
        await _genreService.AddGenre(genre);
        return Ok("Genre added");
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGenre(long id)
    {
        await _genreService.RemoveGenre(id);
        return Ok("Genre deleted");
    }

    [HttpPatch("{id:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateGenre(long id, [FromBody] GenreDto.Request dto)
    {
       await _genreService.UpdateGenre(id, dto);
       return Ok("Genre updated");
    }

}