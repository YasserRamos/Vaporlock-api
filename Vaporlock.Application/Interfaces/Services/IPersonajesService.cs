using Vaporlock.Application.DTOs.Personajes;

namespace Vaporlock.Application.Interfaces.Services;

public interface IPersonajesService
{
    Task<IEnumerable<PersonajeDto>> ObtenerPublicadosAsync();
    Task<PersonajeDto?> ObtenerPorSlugAsync(string slug);
}