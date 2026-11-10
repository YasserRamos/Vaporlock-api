using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.DTOs.MiembrosEquipo;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/miembros-equipo")]
public class MiembrosEquipoController : ControllerBase
{
    private readonly IMiembrosEquipoService _service;

    public MiembrosEquipoController(IMiembrosEquipoService service)
    {
        _service = service;
    }

    // GET: api/miembros-equipo
    // Consulta pública: solamente miembros publicados.
    [HttpGet]
    public async Task<IActionResult> ObtenerPublicados()
    {
        var miembros = await _service.ObtenerPublicadosAsync();

        return Ok(miembros);
    }

    // GET: api/miembros-equipo/admin
    // Consulta administrativa: todos los miembros.
    [HttpGet("admin")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var miembros = await _service.ObtenerTodosAsync();

        return Ok(miembros);
    }

    // GET: api/miembros-equipo/admin/5
    [HttpGet("admin/{id:int}", Name = "ObtenerMiembroEquipoPorId")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });
        }

        var miembro = await _service.ObtenerPorIdAsync(id);

        if (miembro is null)
            return NotFound();

        return Ok(miembro);
    }

    // POST: api/miembros-equipo
    [HttpPost]
    public async Task<IActionResult> Crear(
        [FromBody] CrearMiembroEquipoDto dto)
    {
        int id = await _service.CrearAsync(dto);

        return CreatedAtRoute(
            "ObtenerMiembroEquipoPorId",
            new { id },
            new
            {
                id,
                mensaje = "Miembro del equipo creado correctamente."
            }
        );
    }

    // PUT: api/miembros-equipo/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarMiembroEquipoDto dto)
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

    // PATCH: api/miembros-equipo/5
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> ActualizarParcial(
        int id,
        [FromBody] ActualizarMiembroEquipoParcialDto dto)
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

    // DELETE: api/miembros-equipo/5
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