using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/personajes")]
public class PersonajesController : ControllerBase
{
    private readonly IPersonajesService _service;

    public PersonajesController(IPersonajesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPublicados()
    {
        var personajes = await _service.ObtenerPublicadosAsync();
        return Ok(personajes);
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

        var personaje = await _service.ObtenerPorSlugAsync(slug);

        if (personaje is null)
        {
            return NotFound();
        }

        return Ok(personaje);
    }
}