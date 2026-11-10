using Vaporlock.Application.DTOs.Noticias;

namespace Vaporlock.Application.Interfaces.Services;

public interface INoticiasService
{
    // Consultas públicas
    Task<IEnumerable<NoticiaDto>> ObtenerPublicadasAsync();
    Task<NoticiaDto?> ObtenerPorSlugAsync(string slug);

    // Consultas administrativas
    Task<IEnumerable<NoticiaDto>> ObtenerTodasAsync();
    Task<NoticiaDto?> ObtenerPorIdAsync(int id);

    // Operaciones CRUD
    Task<int> CrearAsync(CrearNoticiaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarNoticiaDto dto);
    Task<bool> ActualizarParcialAsync(int id, ActualizarNoticiaParcialDto dto);
    Task<bool> EliminarAsync(int id);
}