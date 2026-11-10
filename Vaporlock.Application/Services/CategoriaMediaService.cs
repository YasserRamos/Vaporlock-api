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

    public Task<int> CrearAsync(CrearCategoriaMediaDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        return _repository.CrearAsync(dto);
    }

    public Task<bool> ActualizarAsync(int id, ActualizarCategoriaMediaDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        return _repository.ActualizarAsync(id, dto);
    }

    public Task<bool> ActualizarParcialAsync(
        int id, ActualizarCategoriaMediaParcialDto dto)
    {
        return _repository.ActualizarParcialAsync(id, dto);
    }

    public Task<bool> EliminarAsync(int id)
    {
        return _repository.EliminarAsync(id);
    }
}