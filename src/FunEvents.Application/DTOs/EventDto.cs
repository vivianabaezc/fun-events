namespace FunEvents.Application.DTOs;

public class EventDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Capacity { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = string.Empty;

    public Guid VenueId { get; set; }

    public string VenueName { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}