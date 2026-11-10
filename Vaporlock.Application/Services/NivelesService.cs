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

    public Task<IEnumerable<NivelDto>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<NivelDto?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task<int> CrearAsync(CrearNivelDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        dto.Slug = dto.Slug.Trim().ToLowerInvariant();

        return _repository.CrearAsync(dto);
    }

    public Task<bool> ActualizarAsync(int id, ActualizarNivelDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        dto.Slug = dto.Slug.Trim().ToLowerInvariant();

        return _repository.ActualizarAsync(id, dto);
    }

    public Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarNivelParcialDto dto)
    {
        return _repository.ActualizarParcialAsync(id, dto);
    }

    public Task<bool> EliminarAsync(int id)
    {
        return _repository.EliminarAsync(id);
    }
}