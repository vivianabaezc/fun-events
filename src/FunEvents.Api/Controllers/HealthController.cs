using Microsoft.AspNetCore.Mvc;

namespace FunEvents.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            application = "FunEvents.Api"
        });
    }
}