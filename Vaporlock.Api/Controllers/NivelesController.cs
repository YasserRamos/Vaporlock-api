using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/niveles")]
public class NivelesController : ControllerBase
{
    private readonly INivelesService _service;

    public NivelesController(INivelesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPublicados()
    {
        var niveles = await _service.ObtenerPublicadosAsync();
        return Ok(niveles);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> ObtenerPorSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return BadRequest(new
            {
                mensaje = "El slug es obligatorio."
            });
        }

        var nivel = await _service.ObtenerPorSlugAsync(slug);

        if (nivel is null)
        {
            return NotFound();
        }

        return Ok(nivel);
    }
}