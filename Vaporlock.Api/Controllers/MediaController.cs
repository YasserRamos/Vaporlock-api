using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var media = await _mediaService.ObtenerTodosAsync();

        return Ok(media);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El identificador debe ser mayor que cero."
            });
        }

        var media = await _mediaService.ObtenerPorIdAsync(id);

        if (media is null)
        {
            return NotFound();
        }

        return File(
            media.Datos,
            media.ContentType,
            enableRangeProcessing: false
        );
    }
}