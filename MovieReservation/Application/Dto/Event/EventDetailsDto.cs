using Application.Dto.Movie;
using Application.Dto.Venue;

namespace Application.Dto.Event;

public record EventDetailsDto(
    string EventName,
    DateTime StartDate, 
    DateTime EndDate,
    VenueDto Venue,
    IEnumerable<MovieDto.MinimumResponse> Movies);