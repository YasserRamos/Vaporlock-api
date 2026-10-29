using Vaporlock.Application.DTOs.Historias;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface IHistoriasRepository
{
    Task<IEnumerable<HistoriaDto>> ObtenerPublicadasAsync();
    Task<HistoriaDto?> ObtenerPorSlugAsync(string slug);
}