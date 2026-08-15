using FunEvents.Domain.Entities;

namespace FunEvents.Application.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id);

    Task<int> GetReservedQuantityForEventAsync(Guid eventId);

    Task AddAsync(Reservation reservation);
}
