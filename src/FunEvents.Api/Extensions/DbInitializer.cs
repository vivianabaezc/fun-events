using FunEvents.Domain.Entities;
using FunEvents.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Api.Extensions;

public static class DbInitializer
{
    public static void MigrateAndSeed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FunEventsDbContext>();

        context.Database.Migrate();

        if (context.Users.Any())
            return;

        var venue = new Venue("Teatro Colón", "Cerrito 628, Buenos Aires", 200);
        var category = new Category("Teatro");
        var user = new User("Juan", "Perez", "juan.perez@example.com");
        var eventEntity = new Event(
            "Obra de prueba",
            "Evento sembrado para pruebas del cliente de consola",
            DateTime.UtcNow.AddDays(30),
            DateTime.UtcNow.AddDays(30).AddHours(2),
            100,
            50m,
            venue.Id,
            category.Id);
        eventEntity.Publish();

        context.Venues.Add(venue);
        context.Categories.Add(category);
        context.Users.Add(user);
        context.Events.Add(eventEntity);
        context.SaveChanges();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation(
            "Datos de prueba creados. EventId={EventId} UserId={UserId}",
            eventEntity.Id,
            user.Id);
    }
}
