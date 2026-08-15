using FunEvents.Domain.Enums;

namespace FunEvents.Domain.Entities;

public class Ticket
{
    public Guid Id { get; private set; }

    public Guid EventId { get; private set; }

    public Guid? ReservationId { get; private set; }

    public decimal Price { get; private set; }

    public TicketStatus Status { get; private set; }

    public Event? Event { get; private set; }

    public Reservation? Reservation { get; private set; }

    private Ticket()
    {
    }

    public Ticket(
        Guid eventId,
        decimal price)
    {
        if (price < 0)
            throw new ArgumentException(
                "Ticket price cannot be negative.");

        Id = Guid.NewGuid();
        EventId = eventId;
        Price = price;
        Status = TicketStatus.Available;
    }

    public void Reserve(Guid reservationId)
    {
        if (Status != TicketStatus.Available)
            throw new InvalidOperationException(
                "Ticket is not available.");

        ReservationId = reservationId;
        Status = TicketStatus.Reserved;
    }

    public void Sell()
    {
        if (Status != TicketStatus.Reserved)
            throw new InvalidOperationException(
                "Only reserved tickets can be sold.");

        Status = TicketStatus.Sold;
    }

    public void Cancel()
    {
        if (Status == TicketStatus.Sold)
            throw new InvalidOperationException(
                "A sold ticket cannot be cancelled.");

        Status = TicketStatus.Cancelled;
    }
}