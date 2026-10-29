using Vaporlock.Application.DTOs.Media;

namespace Vaporlock.Application.Interfaces.Services;

public interface ICategoriaMediaService
{
    Task<IEnumerable<CategoriaMediaDto>> ObtenerTodosAsync();
    Task<CategoriaMediaDto?> ObtenerPorIdAsync(int id);
}