using Vaporlock.Application.DTOs.Media;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class CategoriaMediaService : ICategoriaMediaService
{
    private readonly ICategoriaMediaRepository _repository;

    public CategoriaMediaService(ICategoriaMediaRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CategoriaMediaDto>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<CategoriaMediaDto?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }
}