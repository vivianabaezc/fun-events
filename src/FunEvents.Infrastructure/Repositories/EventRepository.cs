using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;
using FunEvents.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly FunEventsDbContext _context;

    public EventRepository(FunEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _context.Events
            .Include(e => e.Venue)
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyCollection<Event>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.Venue)
            .Include(e => e.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Event eventEntity)
    {
        await _context.Events.AddAsync(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event eventEntity)
    {
        _context.Events.Update(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Event eventEntity)
    {
        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync();
    }
}