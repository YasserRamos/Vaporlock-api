using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<IActionResult> ObtenerPublicadas()
    {
        var historias = await _service.ObtenerPublicadasAsync();
        return Ok(historias);
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

        var historia = await _service.ObtenerPorSlugAsync(slug);

        if (historia is null)
        {
            return NotFound();
        }

        return Ok(historia);
    }
}