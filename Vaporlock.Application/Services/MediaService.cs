using Vaporlock.Application.DTOs.Media;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class MediaService : IMediaService
{
    private readonly IMediaRepository _mediaRepository;

    public MediaService(IMediaRepository mediaRepository)
    {
        _mediaRepository = mediaRepository;
    }

    public async Task<IEnumerable<MediaDto>> ObtenerTodosAsync()
    {
        return await _mediaRepository.ObtenerTodosAsync();
    }

    public async Task<MediaContenidoDto?> ObtenerPorIdAsync(int id)
    {
        return await _mediaRepository.ObtenerPorIdAsync(id);
    }

    public async Task<int> CrearAsync(
        string nombreArchivo,
        string contentType,
        byte[] datos,
        string? textoAlternativo)
    {
        return await _mediaRepository.CrearAsync(
            nombreArchivo,
            contentType,
            datos,
            textoAlternativo
        );
    }
}