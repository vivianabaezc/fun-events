namespace FunEvents.Application.DTOs;

public class CreateEventDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Capacity { get; set; }

    public decimal Price { get; set; }

    public Guid VenueId { get; set; }

    public Guid CategoryId { get; set; }
}