using Application.Abstraction;
using Application.Dto.Venue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/venues")]
[Authorize(Roles = "Admin")]
public class VenueController : ControllerBase
{
    private readonly IVenueService _venueService;

    public VenueController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpGet]
    public async Task<IActionResult> GetVenues()
    {
        return Ok(await _venueService.GetAllVenues());
    }

    [HttpGet]
    [Route("{id:long}")]
    public async Task<IActionResult> GetVenue(long id)
    {
        return Ok(await _venueService.GetVenue(id));
    }

    [HttpPut]
    [Route("{id:long}")]
    public async Task<IActionResult> UpdateVenue(long id, VenueDto dto)
    {
        await _venueService.UpdateVenue(id, dto);
        return Ok(new {message = "Venue updated successfully"});
    }
}