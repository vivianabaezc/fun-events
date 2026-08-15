using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;
using FunEvents.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FunEventsDbContext _context;

    public UserRepository(FunEventsDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IReadOnlyCollection<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
