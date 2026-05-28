using Application.Abstraction;
using Application.Dto;
using Application.Dto.Event;
using Application.Dto.Reservation;
using Application.Dto.User;
using Application.Dto.Venue;
using Domain.Abstractions;

namespace Application.Services;

public class ReservationsService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationsService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<PagedResult<UserReservationDto>> GetReservations(int page, int pageSize)
    {
        var reservations = await _reservationRepository.GetAllReservationsAsync(page, pageSize);

        var allReservations = reservations.Count;
        
        var allPages = (int)Math.Ceiling(allReservations / (double)pageSize);

        var result = reservations.Select(e => new UserReservationDto()
        {
            Email = e.User.Email,
            FirstName = e.User.FirstName,
            LastName = e.User.LastName,
            PhoneNumber = e.User.PhoneNumber,
            Reservations = reservations.Select(x=>
                new ReservationDto(x.CreatedAt,
                    x.ReservationCode,
                    x.TotalPrice,
                new EventDto(x.Event.Name,
                    x.Event.StartDate,
                    x.Event.EndDate,
                    new VenueDto(x.Event.Venue.Name,
                        x.Event.Venue.City,
                        x.Event.Venue.PostalCode,
                        x.Event.Venue.Street))))
                .ToList()
        }).ToList();


        var pagedResult = new PagedResult<UserReservationDto>()
        {
            Items = result,
            Page = page,
            PageSize = pageSize,
            TotalItems = allReservations,
            TotalPages = allPages
        };

        
        return pagedResult;
    }
}