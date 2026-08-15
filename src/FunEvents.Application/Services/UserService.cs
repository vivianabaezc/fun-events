using FunEvents.Application.DTOs;
using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;

namespace FunEvents.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return null;

        return MapToDto(user);
    }

    public async Task<IReadOnlyCollection<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users
            .Select(MapToDto)
            .ToList();
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = new User(
            dto.FirstName,
            dto.LastName,
            dto.Email);

        await _userRepository.AddAsync(user);

        return MapToDto(user);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}
