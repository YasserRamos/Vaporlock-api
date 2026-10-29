using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/categorias-media")]
public class CategoriasMediaController : ControllerBase
{
    private readonly ICategoriaMediaService _service;

    public CategoriasMediaController(ICategoriaMediaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await _service.ObtenerTodosAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "El identificador debe ser mayor que cero." });

        var categoria = await _service.ObtenerPorIdAsync(id);

        if (categoria is null)
            return NotFound();

        return Ok(categoria);
    }
}