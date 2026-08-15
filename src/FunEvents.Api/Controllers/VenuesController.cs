using FunEvents.Application.DTOs;
using FunEvents.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunEvents.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    private readonly VenueService _venueService;

    public VenuesController(VenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<VenueDto>>> GetAll()
    {
        var venues = await _venueService.GetAllAsync();

        return Ok(venues);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VenueDto>> GetById(Guid id)
    {
        var venue = await _venueService.GetByIdAsync(id);

        if (venue is null)
            return NotFound();

        return Ok(venue);
    }

    [HttpPost]
    public async Task<ActionResult<VenueDto>> Create(
        CreateVenueDto dto)
    {
        var venue = await _venueService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = venue.Id },
            venue);
    }
}