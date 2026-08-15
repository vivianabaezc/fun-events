using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;
using FunEvents.Domain.Enums;
using FunEvents.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly FunEventsDbContext _context;

    public ReservationRepository(FunEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> GetReservedQuantityForEventAsync(Guid eventId)
    {
        return await _context.Reservations
            .Where(r => r.EventId == eventId && r.Status != ReservationStatus.Cancelled)
            .SumAsync(r => (int?)r.Quantity) ?? 0;
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }
}
