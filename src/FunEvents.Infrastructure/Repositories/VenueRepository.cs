using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;
using FunEvents.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Infrastructure.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly FunEventsDbContext _context;

    public VenueRepository(FunEventsDbContext context)
    {
        _context = context;
    }

    public async Task<Venue?> GetByIdAsync(Guid id)
    {
        return await _context.Venues
            .Include(v => v.Events)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IReadOnlyCollection<Venue>> GetAllAsync()
    {
        return await _context.Venues
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Venue venue)
    {
        await _context.Venues.AddAsync(venue);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Venue venue)
    {
        _context.Venues.Update(venue);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Venue venue)
    {
        _context.Venues.Remove(venue);
        await _context.SaveChangesAsync();
    }
}