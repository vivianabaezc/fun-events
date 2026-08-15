using FunEvents.Application.DTOs;
using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Application.Services;

public class VenueService
{
    private readonly IVenueRepository _venueRepository;

    public VenueService(IVenueRepository venueRepository)
    {
        _venueRepository = venueRepository;
    }

    public async Task<VenueDto?> GetByIdAsync(Guid id)
    {
        var venue = await _venueRepository.GetByIdAsync(id);

        if (venue is null)
            return null;

        return MapToDto(venue);
    }

    public async Task<IReadOnlyCollection<VenueDto>> GetAllAsync()
    {
        var venues = await _venueRepository.GetAllAsync();

        return venues
            .Select(MapToDto)
            .ToList();
    }

    public async Task<VenueDto> CreateAsync(CreateVenueDto dto)
    {
        var venue = new Venue(
            dto.Name,
            dto.Address,
            dto.Capacity);

        await _venueRepository.AddAsync(venue);

        return MapToDto(venue);
    }

    private static VenueDto MapToDto(Venue venue)
    {
        return new VenueDto
        {
            Id = venue.Id,
            Name = venue.Name,
            Address = venue.Address,
            Capacity = venue.Capacity
        };
    }
}