using Vaporlock.Application.DTOs.Historias;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class HistoriasService : IHistoriasService
{
    private readonly IHistoriasRepository _repository;

    public HistoriasService(IHistoriasRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<HistoriaDto>> ObtenerPublicadasAsync()
    {
        return _repository.ObtenerPublicadasAsync();
    }

    public Task<HistoriaDto?> ObtenerPorSlugAsync(string slug)
    {
        return _repository.ObtenerPorSlugAsync(slug);
    }
}