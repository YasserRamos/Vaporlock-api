using Vaporlock.Application.DTOs.Niveles;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class NivelesService : INivelesService
{
    private readonly INivelesRepository _repository;

    public NivelesService(INivelesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<NivelDto>> ObtenerPublicadosAsync()
    {
        return _repository.ObtenerPublicadosAsync();
    }

    public Task<NivelDto?> ObtenerPorSlugAsync(string slug)
    {
        return _repository.ObtenerPorSlugAsync(slug);
    }
}