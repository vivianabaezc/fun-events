using FunEvents.Domain.Entities;

namespace FunEvents.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<User>> GetAllAsync();

    Task AddAsync(User user);
}
