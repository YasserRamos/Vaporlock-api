using Vaporlock.Application.DTOs.Niveles;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface INivelesRepository
{
    // Consultas públicas
    Task<IEnumerable<NivelDto>> ObtenerPublicadosAsync();
    Task<NivelDto?> ObtenerPorSlugAsync(string slug);

    // Consultas administrativas
    Task<IEnumerable<NivelDto>> ObtenerTodosAsync();
    Task<NivelDto?> ObtenerPorIdAsync(int id);

    // Operaciones CRUD
    Task<int> CrearAsync(CrearNivelDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarNivelDto dto);
    Task<bool> ActualizarParcialAsync(int id, ActualizarNivelParcialDto dto);
    Task<bool> EliminarAsync(int id);
}