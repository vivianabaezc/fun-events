namespace FunEvents.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public ICollection<Event> Events { get; private set; } = new List<Event>();

    private Category()
    {
    }

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Category name is required.",
                nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }
}