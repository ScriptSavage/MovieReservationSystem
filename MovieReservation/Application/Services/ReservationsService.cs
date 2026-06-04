using Application.Abstraction;
using Application.Dto;
using Application.Dto.Event;
using Application.Dto.Reservation;
using Application.Dto.User;
using Application.Dto.Venue;
using Application.Exceptions;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class ReservationsService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationsService(
        IReservationRepository reservationRepository,
        IUserRepository userRepository,
        IEventRepository eventRepository,
        ITicketTypeRepository ticketTypeRepository,
        IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _unitOfWork = unitOfWork;
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

public async Task CreateReservationAsync(string userId, CreateReservationDto createReservationDto)
{
    
    var user = await _userRepository.GetUserAsync(userId);

    if (user is null)
    {
        throw new DoesNotExistsException("User not found");
    }

    var eventEntity = await _eventRepository.GetEventTicketTypes(createReservationDto.EventId);

    if (eventEntity is null)
    {
        throw new DoesNotExistsException("Event not found");
    }

    if (eventEntity.StartDate >= eventEntity.EndDate)
    {
        throw new InvalidDataException("Event start date cannot be greater than or equal to end date");
    }

    if (eventEntity.StartDate <= DateTime.UtcNow)
    {
        throw new InvalidDataException("Cannot create reservation for past event");
    }

    if (createReservationDto.Tickets is null || createReservationDto.Tickets.Count == 0)
    {
        throw new ArgumentException("Tickets cannot be empty");
    }

    var groupedTickets = createReservationDto.Tickets
        .GroupBy(t => t.TicketTypeId)
        .Select(g => new
        {
            TicketTypeId = g.Key,
            Quantity = g.Sum(x => x.Quantity)
        })
        .ToList();

    foreach (var ticket in groupedTickets)
    {
        if (ticket.Quantity <= 0)
        {
            throw new ArgumentException("Ticket quantity must be greater than zero");
        }
    }

    var reservation = new Reservation
    {
        UserId = userId,
        EventId = eventEntity.EventId,
        CreatedAt = DateTime.UtcNow
    };

    decimal totalPrice = 0;

    foreach (var ticket in groupedTickets)
    {
        var ticketType = eventEntity.TicketTypes
            .FirstOrDefault(e => e.TicketTypeId == ticket.TicketTypeId);

        if (ticketType is null)
        {
            throw new DoesNotExistsException("Ticket type not found for this event");
        }

        var alreadyReserved = await _ticketTypeRepository
            .CountActiveTicketsByTicketTypeIdAsync(ticket.TicketTypeId);

        var available = ticketType.Capacity - alreadyReserved;

        if (ticket.Quantity > available)
        {
            throw new Exception($"Not enough tickets available for {ticketType.Name}");
        }

        for (int i = 0; i < ticket.Quantity; i++)
        {
            reservation.Tickets.Add(new Ticket
            {
                TicketTypeId = ticketType.TicketTypeId
            });
        }

        totalPrice += ticketType.Price * ticket.Quantity;
    }

    reservation.TotalPrice = totalPrice;

    await _reservationRepository.AddNewReservationAsync(reservation);
    await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }


}