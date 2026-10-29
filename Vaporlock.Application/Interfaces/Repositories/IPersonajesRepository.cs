using Vaporlock.Application.DTOs.Personajes;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface IPersonajesRepository
{
    Task<IEnumerable<PersonajeDto>> ObtenerPublicadosAsync();
    Task<PersonajeDto?> ObtenerPorSlugAsync(string slug);
}