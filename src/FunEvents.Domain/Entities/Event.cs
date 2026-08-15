using FunEvents.Domain.Enums;

namespace FunEvents.Domain.Entities;

public class Event
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public int Capacity { get; private set; }

    public decimal Price { get; private set; }

    public EventStatus Status { get; private set; }

    public Guid VenueId { get; private set; }

    public Guid CategoryId { get; private set; }

    public Venue? Venue { get; private set; }

    public Category? Category { get; private set; }

    private Event()
    {
    }

    public Event(
        string name,
        string description,
        DateTime startDate,
        DateTime endDate,
        int capacity,
        decimal price,
        Guid venueId,
        Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Event name is required.", nameof(name));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.");

        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
        Price = price;
        VenueId = venueId;
        CategoryId = categoryId;
        Status = EventStatus.Draft;
    }

    public void Publish()
    {
        if (Status == EventStatus.Cancelled)
            throw new InvalidOperationException(
                "A cancelled event cannot be published.");

        Status = EventStatus.Published;
    }

    public void Cancel()
    {
        if (Status == EventStatus.Completed)
            throw new InvalidOperationException(
                "A completed event cannot be cancelled.");

        Status = EventStatus.Cancelled;
    }

    public void Complete()
    {
        if (Status != EventStatus.Published)
            throw new InvalidOperationException(
                "Only published events can be completed.");

        Status = EventStatus.Completed;
    }
}