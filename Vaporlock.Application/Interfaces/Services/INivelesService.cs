using Vaporlock.Application.DTOs.Niveles;

namespace Vaporlock.Application.Interfaces.Services;

public interface INivelesService
{
    Task<IEnumerable<NivelDto>> ObtenerPublicadosAsync();
    Task<NivelDto?> ObtenerPorSlugAsync(string slug);
}