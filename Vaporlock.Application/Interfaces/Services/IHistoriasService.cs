using Vaporlock.Application.DTOs.Historias;

namespace Vaporlock.Application.Interfaces.Services;

public interface IHistoriasService
{
    Task<IEnumerable<HistoriaDto>> ObtenerPublicadasAsync();
    Task<HistoriaDto?> ObtenerPorSlugAsync(string slug);
}