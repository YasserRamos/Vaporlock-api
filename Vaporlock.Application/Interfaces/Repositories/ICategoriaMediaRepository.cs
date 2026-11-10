using Vaporlock.Application.DTOs.Media;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface ICategoriaMediaRepository
{
    Task<IEnumerable<CategoriaMediaDto>> ObtenerTodosAsync();
    Task<CategoriaMediaDto?> ObtenerPorIdAsync(int id);
    Task<int> CrearAsync(CrearCategoriaMediaDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarCategoriaMediaDto dto);
    Task<bool> ActualizarParcialAsync(int id, ActualizarCategoriaMediaParcialDto dto);
    Task<bool> EliminarAsync(int id);
}