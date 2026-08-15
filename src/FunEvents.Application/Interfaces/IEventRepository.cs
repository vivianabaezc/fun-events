using FunEvents.Domain.Entities;

namespace FunEvents.Application.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<Event>> GetAllAsync();

    Task AddAsync(Event eventEntity);

    Task UpdateAsync(Event eventEntity);

    Task DeleteAsync(Event eventEntity);
}