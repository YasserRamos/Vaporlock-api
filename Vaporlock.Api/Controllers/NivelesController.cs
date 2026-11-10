using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.DTOs.Niveles;
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

    // GET: api/niveles
    // Consulta pública: solamente niveles publicados.
    [HttpGet]
    public async Task<IActionResult> ObtenerPublicados()
    {
        var niveles = await _service.ObtenerPublicadosAsync();

        return Ok(niveles);
    }

    // GET: api/niveles/slug/primer-nivel
    // Consulta pública por slug.
    [HttpGet("slug/{slug}")]
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
            return NotFound();

        return Ok(nivel);
    }

    // GET: api/niveles/admin
    // Consulta administrativa: todos los niveles.
    [HttpGet("admin")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var niveles = await _service.ObtenerTodosAsync();

        return Ok(niveles);
    }

    // GET: api/niveles/admin/5
    [HttpGet("admin/{id:int}", Name = "ObtenerNivelPorId")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });
        }

        var nivel = await _service.ObtenerPorIdAsync(id);

        if (nivel is null)
            return NotFound();

        return Ok(nivel);
    }

    // POST: api/niveles
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearNivelDto dto)
    {
        int id = await _service.CrearAsync(dto);

        return CreatedAtRoute(
            "ObtenerNivelPorId",
            new { id },
            new
            {
                id,
                mensaje = "Nivel creado correctamente."
            }
        );
    }

    // PUT: api/niveles/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarNivelDto dto)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });
        }

        bool actualizado = await _service.ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    // PATCH: api/niveles/5
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> ActualizarParcial(
        int id,
        [FromBody] ActualizarNivelParcialDto dto)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });
        }

        try
        {
            bool actualizado = await _service.ActualizarParcialAsync(id, dto);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    // DELETE: api/niveles/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });
        }

        bool eliminado = await _service.EliminarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}