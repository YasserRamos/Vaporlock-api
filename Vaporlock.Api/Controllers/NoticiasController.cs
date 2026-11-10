using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.DTOs.Noticias;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/noticias")]
public class NoticiasController : ControllerBase
{
    private readonly INoticiasService _service;

    public NoticiasController(INoticiasService service)
    {
        _service = service;
    }

    // GET: api/noticias
    // Consulta pública: solamente noticias publicadas y disponibles.
    [HttpGet]
    public async Task<IActionResult> ObtenerPublicadas()
    {
        return Ok(await _service.ObtenerPublicadasAsync());
    }

    // GET: api/noticias/admin
    // Consulta administrativa: todas las noticias.
    [HttpGet("admin")]
    public async Task<IActionResult> ObtenerTodas()
    {
        return Ok(await _service.ObtenerTodasAsync());
    }

    // GET: api/noticias/admin/5
    [HttpGet("admin/{id:int}", Name = "ObtenerNoticiaPorId")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });
        }

        var noticia = await _service.ObtenerPorIdAsync(id);

        if (noticia is null)
            return NotFound();

        return Ok(noticia);
    }

    // GET: api/noticias/mi-noticia
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

        var noticia = await _service.ObtenerPorSlugAsync(slug);

        if (noticia is null)
            return NotFound();

        return Ok(noticia);
    }

    // POST: api/noticias
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromBody] CrearNoticiaDto dto)
    {
        int id = await _service.CrearAsync(dto);

        return CreatedAtRoute(
            "ObtenerNoticiaPorId",
            new { id },
            new
            {
                id,
                mensaje = "Noticia creada correctamente."
            }
        );
    }

    // PUT: api/noticias/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarNoticiaDto dto)
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

    // PATCH: api/noticias/5
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> ActualizarParcial(
        int id,
        [FromBody] ActualizarNoticiaParcialDto dto)
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

    // DELETE: api/noticias/5
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