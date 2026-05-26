using System.Data;
using Application.Abstraction;
using Application.Dto;
using Application.Dto.Event;
using Application.Dto.Movie;
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
    private readonly IMovieResepository _movieResepository;

    public EventService(IEventRepository eventRepository, 
        IVenueRepository venueRepository, 
        IUnitOfWork unitOfWork,
        IMovieResepository movieResepository)
    {
        _eventRepository = eventRepository;
        _venueRepository = venueRepository;
        _unitOfWork = unitOfWork;
        _movieResepository = movieResepository;
    }


    public async Task AddNewEventVenueAsync(NewEventDto dto)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable);
        
        var newEvent = new Event()
        {
            Name = dto.Name,
            TotalCapacity = dto.TotalCapacity,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
        };
        
        var newVenue = new Venue()
        {
            Name = dto.Venue.Name,
            Street = dto.Venue.Street,
            City = dto.Venue.City,
            PostalCode = dto.Venue.PostalCode,
        };
        
        try
        {
            newEvent.VenueId = newVenue.VenueId;
           await _eventRepository.AddNewEvent(newEvent);
           await _venueRepository.AddNewVenue(newVenue);
           transaction.Commit();
        }
        catch (Exception e)
        {
            transaction.Rollback();
        }
    }

    public async Task AddMovieToEventAsync(long eventId, long movieId)
    {
        var eventEntity = await _eventRepository.GetEvent(eventId);
        var doesMovieExist = await _movieResepository.DoesMovieExist(movieId);
        if (!doesMovieExist)
        {
            throw new DoesNotExistsException("Movie doesn't exist");
        }

        var movieEntity = await _movieResepository.GetMovieAsync(movieId);
        
        eventEntity.Movies.Add(movieEntity);
    }

    
    public async Task<PagedResult<EventDetailsDto>> GetAllEventsAsync(int page, int pageSize)
    {
        var data = await _eventRepository.GetEvents(page, pageSize);
        
        var totalItems = await _eventRepository.CountEvents();
        var allPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var eventDetailsResult = data.Select(e =>
            new EventDetailsDto(e.Name, e.StartDate, e.EndDate,
                new VenueDto(e.Venue.Name, e.Venue.City, e.Venue.PostalCode,e.Venue.Street),
                e.Movies.Select(x=> new MovieDto.MinimumResponse(x.Title,x.Description))
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

   
}