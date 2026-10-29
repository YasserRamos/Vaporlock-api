using Vaporlock.Application.DTOs.Noticias;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class NoticiasService : INoticiasService
{
    private readonly INoticiasRepository _repository;

    public NoticiasService(INoticiasRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<NoticiaDto>> ObtenerPublicadasAsync()
    {
        return _repository.ObtenerPublicadasAsync();
    }

    public Task<NoticiaDto?> ObtenerPorSlugAsync(string slug)
    {
        return _repository.ObtenerPorSlugAsync(slug);
    }
}