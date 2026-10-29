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

    public Task<IEnumerable<MediaDto>> ObtenerTodosAsync()
    {
        return _mediaRepository.ObtenerTodosAsync();
    }

    public Task<MediaContenidoDto?> ObtenerPorIdAsync(int id)
    {
        return _mediaRepository.ObtenerPorIdAsync(id);
    }
}