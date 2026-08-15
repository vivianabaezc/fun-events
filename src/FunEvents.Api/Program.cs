using FunEvents.Api.Extensions;
using FunEvents.Application.Interfaces;
using FunEvents.Application.Services;
using FunEvents.Infrastructure.Persistence;
using FunEvents.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FunEventsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FunEventsDatabase")));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<VenueService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReservationService>();

var app = builder.Build();

DbInitializer.MigrateAndSeed(app);

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();