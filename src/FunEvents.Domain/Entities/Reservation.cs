using FunEvents.Domain.Enums;

namespace FunEvents.Domain.Entities;

public class Reservation
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid EventId { get; private set; }

    public int Quantity { get; private set; }

    public decimal TotalPrice { get; private set; }

    public ReservationStatus Status { get; private set; }

    public User? User { get; private set; }

    public Event? Event { get; private set; }

    private Reservation()
    {
    }

    public Reservation(
        Guid userId,
        Guid eventId,
        int quantity,
        decimal ticketPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        if (ticketPrice < 0)
            throw new ArgumentException(
                "Ticket price cannot be negative.");

        Id = Guid.NewGuid();
        UserId = userId;
        EventId = eventId;
        Quantity = quantity;
        TotalPrice = quantity * ticketPrice;
        Status = ReservationStatus.Pending;
    }

    public void Confirm()
    {
        if (Status != ReservationStatus.Pending)
            throw new InvalidOperationException(
                "Only pending reservations can be confirmed.");

        Status = ReservationStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == ReservationStatus.Cancelled)
            return;

        Status = ReservationStatus.Cancelled;
    }
}