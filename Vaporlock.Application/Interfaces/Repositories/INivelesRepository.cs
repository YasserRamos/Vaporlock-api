using Vaporlock.Application.DTOs.Niveles;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface INivelesRepository
{
    Task<IEnumerable<NivelDto>> ObtenerPublicadosAsync();
    Task<NivelDto?> ObtenerPorSlugAsync(string slug);
}