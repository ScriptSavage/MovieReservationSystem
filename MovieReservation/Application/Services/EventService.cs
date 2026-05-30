using System.Data;
using Application.Abstraction;
using Application.Dto;
using Application.Dto.Event;
using Application.Dto.Movie;
using Application.Dto.TicketType;
using Application.Dto.Venue;
using Application.Exceptions;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IVenueRepository _venueRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMovieResepository _movieRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;

    public EventService(IEventRepository eventRepository, 
        IVenueRepository venueRepository, 
        IUnitOfWork unitOfWork,
        IMovieResepository movieRepository,
        ITicketTypeRepository ticketTypeRepository)
    {
        _eventRepository = eventRepository;
        _venueRepository = venueRepository;
        _unitOfWork = unitOfWork;
        _movieRepository = movieRepository;
        _ticketTypeRepository = ticketTypeRepository;
    }


    public async Task AddNewEventVenueAsync(NewEventDto dto)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable);
        
        try
        {
            var newVenue = new Venue()
            {
                Name = dto.Venue.Name,
                Street = dto.Venue.Street,
                City = dto.Venue.City,
                PostalCode = dto.Venue.PostalCode,
            };
        
            var newEvent = new Event()
            {
                Name = dto.Name,
                TotalCapacity = dto.TotalCapacity,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Venue = newVenue
            };
            
           await _eventRepository.AddNewEvent(newEvent);
           await _venueRepository.AddNewVenue(newVenue);
           await _unitOfWork.SaveChangesAsync(CancellationToken.None);
           transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
            Console.WriteLine(e.Message);
        }
    }

    public async Task AddMovieToEventAsync(long eventId, long movieId)
    {
        var eventEntity = await _eventRepository.GetEvent(eventId);
        var doesMovieExist = await _movieRepository.DoesMovieExist(movieId);
        if (!doesMovieExist)
        {
            throw new DoesNotExistsException("Movie doesn't exist");
        }

        var movieEntity = await _movieRepository.GetMovieAsync(movieId);
        
        eventEntity.Movies.Add(movieEntity);
    }

    
    public async Task<PagedResult<EventDetailsDto>> GetAllEventsAsync(int page, int pageSize)
    {
        var data = await _eventRepository.GetEvents(page, pageSize);
        
        var totalItems = await _eventRepository.CountEvents();
        var allPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var eventDetailsResult = data.Select(e =>
            new EventDetailsDto(e.Name, 
                e.StartDate, 
                e.EndDate,
                new VenueDto(e.Venue.Name, 
                    e.Venue.City, 
                    e.Venue.PostalCode,
                    e.Venue.Street),
                e.Movies.Select(x=> 
                        new MovieDto.MinimumResponse(x.Title,x.Description))
                    .ToList()))
            .ToList();

        
        var result = new PagedResult<EventDetailsDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = allPages,
            Items = eventDetailsResult
        };
        
        return result;
    }

    public async Task<EventDetailsDto> GetEventDetailsAsync(long eventId)
    {
        var doesEventExist = await _eventRepository.DoesEventExist(eventId);
        if (!doesEventExist)
        {
            throw new DoesNotExistsException("Event doesn't exist");
        }

        var eventEntity = await _eventRepository.GetEventDetails(eventId);

        var enventDetails = new EventDetailsDto(eventEntity.Name, 
            eventEntity.StartDate, 
            eventEntity.EndDate,
            new VenueDto(eventEntity.Venue.Name, 
                eventEntity.Venue.City, 
                eventEntity.Venue.PostalCode,
                eventEntity.Venue.Street),
            eventEntity.Movies.Select(x => 
                new MovieDto.MinimumResponse(x.Title, x.Description)));
        
        return enventDetails;
    }

    public async Task DeleteEventAsync(long eventId)
    {
        var eventEntity = await _eventRepository.GetEvent(eventId);
        
        if (eventEntity == null)
        {
            throw new DoesNotExistsException("Event doesn't exist");
        }

        await _eventRepository.DeleteEvent(eventEntity);
    }

    public async Task UpdateEventDetailsAsync(long eventId ,EventDto dto)
    {
        var doesEventExist = await _eventRepository.DoesEventExist(eventId);
        if (!doesEventExist)
        {
            throw new DoesNotExistsException("Event doesn't exist");
        }

        var eventEntity = await _eventRepository.GetEvent(eventId);

        if (string.IsNullOrWhiteSpace(dto.EventName)) eventEntity.Name = dto.EventName;
        if (dto.StartDate >= dto.EndDate || eventEntity.StartDate != default) eventEntity.StartDate = dto.StartDate;
        if (dto.EndDate <= dto.StartDate || eventEntity.StartDate != default) eventEntity.EndDate = dto.EndDate;
        
        if (string.IsNullOrWhiteSpace(dto.Venue.Name)) eventEntity.Venue.Name = dto.Venue.Name;
        if (string.IsNullOrWhiteSpace(dto.Venue.City)) eventEntity.Venue.City = dto.Venue.City;
        if (string.IsNullOrWhiteSpace(dto.Venue.PostalCode)) eventEntity.Venue.PostalCode = dto.Venue.PostalCode;
        if (string.IsNullOrWhiteSpace(dto.Venue.Street)) eventEntity.Venue.Street = dto.Venue.Street;

            
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AddTicketTypesToEventAsync(long eventId, TicketTypeDto dto)
    {
        var doesEventExist = await _eventRepository.DoesEventExist(eventId);

        if (!doesEventExist)
        {
            throw new DoesNotExistsException("Event doesn't exist");
        }
        
        var eventEntity = await _eventRepository.GetEvent(eventId);

        var newTicketType = new TicketType()
        {
            Name = dto.Name,
            Price = dto.Price,
            Capacity = dto.Capacity
        };

        eventEntity.TicketTypes.Add(newTicketType);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<EventTicketTypesDto> GetEventTicketTypesAsync(long eventId)
    {
        var data = await _eventRepository.GetEventTicketTypes(eventId);
        
        var result = new EventTicketTypesDto(
            data.Name,
            data.TicketTypes.Select(e=> 
                new TicketTypeDto(e.Name,
                    e.Price,
                    e.Capacity)).ToList());
        
        return result;
    }

    public async Task DeleteTicketTypeAsync(long eventId, long ticketTypeId)
    {
        var doesEventExists = await _eventRepository.DoesEventExist(eventId);
        if (!doesEventExists)
        {
            throw new DoesNotExistsException("Event doesn't exist");
        }
        
        var doesTicketTypeExist = await _ticketTypeRepository.DoesTicketTypeExist(ticketTypeId);
        if (!doesTicketTypeExist)
        {
            throw new DoesNotExistsException("Ticket doesn't exist");
        }

        var eventEntity = await _eventRepository.GetEvent(eventId);
        
        var ticketEntity = await _ticketTypeRepository.GetTicketType(ticketTypeId);
        
        eventEntity.TicketTypes.Remove(ticketEntity);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }
}