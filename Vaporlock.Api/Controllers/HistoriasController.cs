using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.DTOs.Historias;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/historias")]
public class HistoriasController : ControllerBase
{
    private readonly IHistoriasService _service;

    public HistoriasController(IHistoriasService service)
    {
        _service = service;
    }

    // GET: api/historias/publicadas
    [HttpGet("publicadas")]
    public async Task<IActionResult> ObtenerPublicadas()
    {
        return Ok(await _service.ObtenerPublicadasAsync());
    }

    // GET: api/historias/slug/mi-historia
    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> ObtenerPorSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return BadRequest(new { mensaje = "El slug es obligatorio." });

        var historia = await _service.ObtenerPorSlugAsync(slug);

        if (historia is null)
            return NotFound();

        return Ok(historia);
    }

    // GET: api/historias
    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        return Ok(await _service.ObtenerTodasAsync());
    }

    // GET: api/historias/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id <= 0)
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });

        var historia = await _service.ObtenerPorIdAsync(id);

        if (historia is null)
            return NotFound();

        return Ok(historia);
    }

    // POST: api/historias
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromBody] CrearHistoriaDto dto)
    {
        int id = await _service.CrearAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id },
            new
            {
                id,
                mensaje = "Historia creada correctamente."
            }
        );
    }

    // PUT: api/historias/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarHistoriaDto dto)
    {
        if (id <= 0)
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });

        bool actualizado = await _service.ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    // PATCH: api/historias/5
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> ActualizarParcial(
        int id,
        [FromBody] ActualizarHistoriaParcialDto dto)
    {
        if (id <= 0)
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });

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
        catch (System.Text.Json.JsonException)
        {
            return BadRequest(new { mensaje = "Uno de los campos contiene un formato inválido." });
        }
    }

    // DELETE: api/historias/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        if (id <= 0)
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });

        bool eliminado = await _service.EliminarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}