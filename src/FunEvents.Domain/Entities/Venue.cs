namespace FunEvents.Domain.Entities;

public class Venue
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public int Capacity { get; private set; }

    public ICollection<Event> Events { get; private set; } = new List<Event>();

    private Venue()
    {
    }

    public Venue(
        string name,
        string address,
        int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Venue name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Venue address is required.", nameof(address));

        if (capacity <= 0)
            throw new ArgumentException("Venue capacity must be greater than zero.");

        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Capacity = capacity;
    }
}