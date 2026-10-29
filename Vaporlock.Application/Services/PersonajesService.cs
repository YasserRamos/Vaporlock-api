using Vaporlock.Application.DTOs.Personajes;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class PersonajesService : IPersonajesService
{
    private readonly IPersonajesRepository _repository;

    public PersonajesService(IPersonajesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonajeDto>> ObtenerPublicadosAsync()
    {
        return _repository.ObtenerPublicadosAsync();
    }

    public Task<PersonajeDto?> ObtenerPorSlugAsync(string slug)
    {
        return _repository.ObtenerPorSlugAsync(slug);
    }
}