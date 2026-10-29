using Vaporlock.Application.DTOs.Media;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface IMediaRepository
{
    Task<IEnumerable<MediaDto>> ObtenerTodosAsync();
    Task<MediaContenidoDto?> ObtenerPorIdAsync(int id);
}