using FunEvents.Application.DTOs;
using FunEvents.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunEvents.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly EventService _eventService;

    public EventsController(EventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<EventDto>>> GetAll()
    {
        var events = await _eventService.GetAllAsync();

        return Ok(events);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventDto>> GetById(Guid id)
    {
        var eventDto = await _eventService.GetByIdAsync(id);

        if (eventDto is null)
            return NotFound();

        return Ok(eventDto);
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> Create(
        CreateEventDto dto)
    {
        try
        {
            var eventDto = await _eventService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = eventDto.Id },
                eventDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}