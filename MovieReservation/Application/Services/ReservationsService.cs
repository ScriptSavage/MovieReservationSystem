using Application.Abstraction;
using Application.Dto;
using Application.Dto.Event;
using Application.Dto.Reservation;
using Application.Dto.User;
using Application.Dto.Venue;
using Application.Exceptions;
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
                    x.TotalPrice)).ToList()
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

    public async Task<UserReservationDetailsDto> GetReservation(long id)
    {
        var doesReservationExists = await _reservationRepository.DoesReservationExistAsync(id);
        if (!doesReservationExists)
        {
            throw new DoesNotExistsException("Reservation not found");
        }

        var reservation = await _reservationRepository.GetReservationAsync(id);

        var result = new UserReservationDetailsDto(
            new UserDetailsDto(reservation.User.Email,
                reservation.User.FirstName,
                reservation.User.LastName,
                reservation.User.PhoneNumber),
            new ReservationDto(reservation.CreatedAt, 
                reservation.ReservationCode, 
                reservation.TotalPrice),
            new EventDto(reservation.Event.Name,
                reservation.Event.StartDate,
                reservation.Event.EndDate,
                new VenueDto(reservation.Event.Venue.Name,
                    reservation.Event.Venue.City,
                    reservation.Event.Venue.PostalCode,
                    reservation.Event.Venue.Street)));
        
        
        return result;
    }
}