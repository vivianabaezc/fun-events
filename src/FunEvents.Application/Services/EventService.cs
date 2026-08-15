using FunEvents.Application.DTOs;
using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Application.Services;

public class EventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IVenueRepository _venueRepository;

    public EventService(
        IEventRepository eventRepository,
        IVenueRepository venueRepository)
    {
        _eventRepository = eventRepository;
        _venueRepository = venueRepository;
    }

    public async Task<EventDto?> GetByIdAsync(Guid id)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(id);

        if (eventEntity is null)
            return null;

        return MapToDto(eventEntity);
    }

    public async Task<IReadOnlyCollection<EventDto>> GetAllAsync()
    {
        var events = await _eventRepository.GetAllAsync();

        return events
            .Select(MapToDto)
            .ToList();
    }

    public async Task<EventDto> CreateAsync(CreateEventDto dto)
    {
        var venue = await _venueRepository.GetByIdAsync(dto.VenueId);

        if (venue is null)
            throw new InvalidOperationException(
                "The specified venue does not exist.");

        if (dto.Capacity > venue.Capacity)
            throw new InvalidOperationException(
                "Event capacity cannot exceed venue capacity.");

        var eventEntity = new Event(
            dto.Name,
            dto.Description,
            dto.StartDate,
            dto.EndDate,
            dto.Capacity,
            dto.Price,
            dto.VenueId,
            dto.CategoryId);

        await _eventRepository.AddAsync(eventEntity);

        return MapToDto(eventEntity);
    }

    private static EventDto MapToDto(Event eventEntity)
    {
        return new EventDto
        {
            Id = eventEntity.Id,
            Name = eventEntity.Name,
            Description = eventEntity.Description,
            StartDate = eventEntity.StartDate,
            EndDate = eventEntity.EndDate,
            Capacity = eventEntity.Capacity,
            Price = eventEntity.Price,
            Status = eventEntity.Status.ToString(),
            VenueId = eventEntity.VenueId,
            VenueName = eventEntity.Venue?.Name ?? string.Empty,
            CategoryId = eventEntity.CategoryId,
            CategoryName = eventEntity.Category?.Name ?? string.Empty
        };
    }
}