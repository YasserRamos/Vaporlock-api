
using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.DTOs.Personajes;
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

    // RUTAS PÚBLICAS

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
                status = 400,
                mensaje = "El slug es obligatorio."
            });
        }

        var personaje = await _service.ObtenerPorSlugAsync(slug);

        if (personaje is null)
            return NotFound(new
            {
                status = 404,
                mensaje = "No se encontró el personaje solicitado."
            });

        return Ok(personaje);
    }

    // RUTAS ADMINISTRATIVAS

    [HttpGet("admin")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var personajes = await _service.ObtenerTodosAsync();

        return Ok(personajes);
    }

    [HttpGet("admin/{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                status = 400,
                mensaje = "El ID debe ser mayor que cero."
            });
        }

        var personaje = await _service.ObtenerPorIdAsync(id);

        if (personaje is null)
        {
            return NotFound(new
            {
                status = 404,
                mensaje = "No se encontró el personaje solicitado."
            });
        }

        return Ok(personaje);
    }

    [HttpPost("admin")]
    public async Task<IActionResult> Crear(
        [FromBody] CrearPersonajeDto dto)
    {
        var id = await _service.CrearAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id },
            new
            {
                status = 201,
                mensaje = "Personaje creado correctamente.",
                pkPersonaje = id
            }
        );
    }

    [HttpPut("admin/{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarPersonajeDto dto)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                status = 400,
                mensaje = "El ID debe ser mayor que cero."
            });
        }

        var actualizado = await _service.ActualizarAsync(id, dto);

        if (!actualizado)
        {
            return NotFound(new
            {
                status = 404,
                mensaje = "No se encontró el personaje que intentas actualizar."
            });
        }

        return Ok(new
        {
            status = 200,
            mensaje = "Personaje actualizado correctamente."
        });
    }

    [HttpPatch("admin/{id:int}")]
    public async Task<IActionResult> ActualizarParcial(
        int id,
        [FromBody] ActualizarPersonajeParcialDto dto)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                status = 400,
                mensaje = "El ID debe ser mayor que cero."
            });
        }

        var actualizado = await _service.ActualizarParcialAsync(id, dto);

        if (!actualizado)
        {
            return NotFound(new
            {
                status = 404,
                mensaje = "No se encontró el personaje que intentas actualizar."
            });
        }

        return Ok(new
        {
            status = 200,
            mensaje = "Personaje actualizado parcialmente."
        });
    }

    [HttpDelete("admin/{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                status = 400,
                mensaje = "El ID debe ser mayor que cero."
            });
        }

        var eliminado = await _service.EliminarAsync(id);

        if (!eliminado)
        {
            return NotFound(new
            {
                status = 404,
                mensaje = "No se encontró el personaje que intentas eliminar."
            });
        }

        return Ok(new
        {
            status = 200,
            mensaje = "Personaje eliminado correctamente."
        });
    }
}
