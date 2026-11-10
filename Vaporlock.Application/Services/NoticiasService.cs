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

    public Task<IEnumerable<NoticiaDto>> ObtenerTodasAsync()
    {
        return _repository.ObtenerTodasAsync();
    }

    public Task<NoticiaDto?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task<int> CrearAsync(CrearNoticiaDto dto)
    {
        dto.Titulo = dto.Titulo.Trim();
        dto.Slug = dto.Slug.Trim().ToLowerInvariant();

        return _repository.CrearAsync(dto);
    }

    public Task<bool> ActualizarAsync(
        int id,
        ActualizarNoticiaDto dto)
    {
        dto.Titulo = dto.Titulo.Trim();
        dto.Slug = dto.Slug.Trim().ToLowerInvariant();

        return _repository.ActualizarAsync(id, dto);
    }

    public Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarNoticiaParcialDto dto)
    {
        return _repository.ActualizarParcialAsync(id, dto);
    }

    public Task<bool> EliminarAsync(int id)
    {
        return _repository.EliminarAsync(id);
    }
}