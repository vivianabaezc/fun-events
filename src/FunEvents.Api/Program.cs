using FunEvents.Application.Interfaces;
using FunEvents.Application.Services;
using FunEvents.Infrastructure.Persistence;
using FunEvents.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<FunEventsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FunEventsDatabase")));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();

builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<VenueService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();