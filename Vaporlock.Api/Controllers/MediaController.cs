
using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/media")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;

    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly string[] AllowedContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/webp"
    ];

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
            return BadRequest("El identificador no es válido.");

        var media = await _mediaService.ObtenerPorIdAsync(id);

        if (media == null || media.Datos == null || media.Datos.Length == 0)
            return NotFound();

        return File(
            media.Datos,
            media.ContentType,
            enableRangeProcessing: false
        );
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Crear(
        IFormFile archivo,
        [FromForm] string? textoAlternativo)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest("Debes seleccionar una imagen.");

        if (archivo.Length > MaxFileSize)
            return BadRequest("La imagen no puede superar los 5 MB.");

        if (!AllowedContentTypes.Contains(archivo.ContentType))
            return BadRequest("Formato no permitido. Utiliza JPG, PNG o WebP.");

        byte[] datos;

        await using (var memoryStream = new MemoryStream())
        {
            await archivo.CopyToAsync(memoryStream);
            datos = memoryStream.ToArray();
        }

        var id = await _mediaService.CrearAsync(
            Path.GetFileName(archivo.FileName),
            archivo.ContentType,
            datos,
            textoAlternativo
        );

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id },
            new
            {
                PkMedia = id,
                Mensaje = "Imagen almacenada correctamente."
            }
        );
    }
}
