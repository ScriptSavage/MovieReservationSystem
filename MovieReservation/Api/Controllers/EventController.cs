using Application.Abstraction;
using Application.Dto.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = await _eventService.GetAllEventsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewEventAsync([FromBody] NewEventDto dto)
    {
        await _eventService.AddNewEventVenueAsync(dto);
        return Ok(new { message = "Event was added successfully" });
    }

    [HttpPost("{eventId:long}/movies/{movieId:long}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewMovieToEventAsync(long eventId, long movieId)
    {
        await _eventService.AddMovieToEventAsync(eventId, movieId);
        return Ok(new { message = "Movie was added successfully" });
    }
}