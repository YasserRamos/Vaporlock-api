using Vaporlock.Application.DTOs.Personajes;

namespace Vaporlock.Application.Interfaces.Services;

public interface IPersonajesService
{
    Task<IEnumerable<PersonajeDto>> ObtenerPublicadosAsync();
    Task<PersonajeDto?> ObtenerPorSlugAsync(string slug);

    Task<IEnumerable<PersonajeDto>> ObtenerTodosAsync();
    Task<PersonajeDto?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(CrearPersonajeDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarPersonajeDto dto);
    Task<bool> ActualizarParcialAsync(int id, ActualizarPersonajeParcialDto dto);
    Task<bool> EliminarAsync(int id);
}