using Application.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetReservationsAsync([FromQuery]int page, [FromQuery]int pageSize)
    {
        var result = await _reservationService.GetReservations(page, pageSize);
        return Ok(result);
    }


    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetReservationAsync(int id)
    {
       var reservationDetails = await _reservationService.GetReservation(id);
        return Ok(reservationDetails);
    }

}