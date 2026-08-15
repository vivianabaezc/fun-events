namespace FunEvents.Application.DTOs;

public class ReservationDto
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }

    public string EventName { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = string.Empty;
}
