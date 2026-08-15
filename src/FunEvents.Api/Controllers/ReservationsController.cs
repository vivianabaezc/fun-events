using FunEvents.Application.DTOs;
using FunEvents.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FunEvents.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationsController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto>> GetById(Guid id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);

        if (reservation is null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> Create(
        CreateReservationDto dto)
    {
        try
        {
            var reservation = await _reservationService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = reservation.Id },
                reservation);
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
