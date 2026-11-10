using Vaporlock.Application.DTOs.Historias;

namespace Vaporlock.Application.Interfaces.Services;

public interface IHistoriasService
{
    // Consultas públicas
    Task<IEnumerable<HistoriaDto>> ObtenerPublicadasAsync();
    Task<HistoriaDto?> ObtenerPorSlugAsync(string slug);

    // Consultas administrativas
    Task<IEnumerable<HistoriaDto>> ObtenerTodasAsync();
    Task<HistoriaDto?> ObtenerPorIdAsync(int id);

    // Operaciones CRUD
    Task<int> CrearAsync(CrearHistoriaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarHistoriaDto dto);
    Task<bool> ActualizarParcialAsync(int id, ActualizarHistoriaParcialDto dto);
    Task<bool> EliminarAsync(int id);
}