using FunEvents.Domain.Entities;

namespace FunEvents.Application.Interfaces;

public interface IVenueRepository
{
    Task<Venue?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<Venue>> GetAllAsync();

    Task AddAsync(Venue venue);

    Task UpdateAsync(Venue venue);

    Task DeleteAsync(Venue venue);
}