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

    public Task<IEnumerable<HistoriaDto>> ObtenerTodasAsync()
    {
        return _repository.ObtenerTodasAsync();
    }

    public Task<HistoriaDto?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task<int> CrearAsync(CrearHistoriaDto dto)
    {
        dto.Titulo = dto.Titulo.Trim();
        dto.Slug = dto.Slug.Trim().ToLowerInvariant();

        return _repository.CrearAsync(dto);
    }

    public Task<bool> ActualizarAsync(int id, ActualizarHistoriaDto dto)
    {
        dto.Titulo = dto.Titulo.Trim();
        dto.Slug = dto.Slug.Trim().ToLowerInvariant();

        return _repository.ActualizarAsync(id, dto);
    }

    public Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarHistoriaParcialDto dto)
    {
        return _repository.ActualizarParcialAsync(id, dto);
    }

    public Task<bool> EliminarAsync(int id)
    {
        return _repository.EliminarAsync(id);
    }
}