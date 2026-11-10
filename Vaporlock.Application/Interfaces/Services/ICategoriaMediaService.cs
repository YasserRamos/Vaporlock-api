using Vaporlock.Application.DTOs.Media;

namespace Vaporlock.Application.Interfaces.Services;

public interface ICategoriaMediaService
{
    Task<IEnumerable<CategoriaMediaDto>> ObtenerTodosAsync();
    Task<CategoriaMediaDto?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(CrearCategoriaMediaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarCategoriaMediaDto dto);
    Task<bool> ActualizarParcialAsync(int id, ActualizarCategoriaMediaParcialDto dto);
    Task<bool> EliminarAsync(int id);
}