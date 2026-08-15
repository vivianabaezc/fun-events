using FunEvents.Application.DTOs;
using FunEvents.Application.Interfaces;
using FunEvents.Domain.Entities;
using FunEvents.Domain.Enums;

namespace FunEvents.Application.Services;

public class ReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IEventRepository eventRepository,
        IUserRepository userRepository)
    {
        _reservationRepository = reservationRepository;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
    }

    public async Task<ReservationDto?> GetByIdAsync(Guid id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);

        if (reservation is null)
            return null;

        return MapToDto(reservation);
    }

    public async Task<ReservationDto> CreateAsync(CreateReservationDto dto)
    {
        if (dto.Quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero.");

        var user = await _userRepository.GetByIdAsync(dto.UserId);

        if (user is null)
            throw new InvalidOperationException("The specified user does not exist.");

        var eventEntity = await _eventRepository.GetByIdAsync(dto.EventId);

        if (eventEntity is null)
            throw new InvalidOperationException("The specified event does not exist.");

        if (eventEntity.Status != EventStatus.Published)
            throw new InvalidOperationException("The event is not open for reservations.");

        var reservedQuantity = await _reservationRepository.GetReservedQuantityForEventAsync(dto.EventId);

        if (reservedQuantity + dto.Quantity > eventEntity.Capacity)
            throw new InvalidOperationException("Not enough available capacity for this event.");

        var reservation = new Reservation(
            dto.UserId,
            dto.EventId,
            dto.Quantity,
            eventEntity.Price);

        await _reservationRepository.AddAsync(reservation);

        var created = await _reservationRepository.GetByIdAsync(reservation.Id);

        return MapToDto(created!);
    }

    private static ReservationDto MapToDto(Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            EventId = reservation.EventId,
            EventName = reservation.Event?.Name ?? string.Empty,
            UserId = reservation.UserId,
            UserName = reservation.User is null
                ? string.Empty
                : $"{reservation.User.FirstName} {reservation.User.LastName}",
            Quantity = reservation.Quantity,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString()
        };
    }
}
