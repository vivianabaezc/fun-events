using FunEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Infrastructure.Persistence;

public class FunEventsDbContext : DbContext
{
    public FunEventsDbContext(
        DbContextOptions<FunEventsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();

    public DbSet<Venue> Venues => Set<Venue>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FunEventsDbContext).Assembly);
    }
}