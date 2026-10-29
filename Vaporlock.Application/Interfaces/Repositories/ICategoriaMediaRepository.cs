using Vaporlock.Application.DTOs.Media;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface ICategoriaMediaRepository
{
    Task<IEnumerable<CategoriaMediaDto>> ObtenerTodosAsync();
    Task<CategoriaMediaDto?> ObtenerPorIdAsync(int id);
}