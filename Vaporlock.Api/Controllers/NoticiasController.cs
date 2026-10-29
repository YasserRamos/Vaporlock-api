using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<IActionResult> ObtenerPublicadas()
    {
        return Ok(await _service.ObtenerPublicadasAsync());
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

        var noticia = await _service.ObtenerPorSlugAsync(slug);

        if (noticia is null)
            return NotFound();

        return Ok(noticia);
    }
}