using Vaporlock.Application.DTOs.Noticias;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface INoticiasRepository
{
    Task<IEnumerable<NoticiaDto>> ObtenerPublicadasAsync();
    Task<NoticiaDto?> ObtenerPorSlugAsync(string slug);
}