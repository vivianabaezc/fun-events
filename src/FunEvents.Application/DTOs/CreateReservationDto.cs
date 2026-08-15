namespace FunEvents.Application.DTOs;

public class CreateReservationDto
{
    public Guid EventId { get; set; }

    public Guid UserId { get; set; }

    public int Quantity { get; set; } = 1;
}
