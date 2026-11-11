using Vaporlock.Application.DTOs.Media;

namespace Vaporlock.Application.Interfaces.Services;

public interface IMediaService
{
    Task<IEnumerable<MediaDto>> ObtenerTodosAsync();

    Task<MediaContenidoDto?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(
        string nombreArchivo,
        string contentType,
        byte[] datos,
        string? textoAlternativo
    );
}