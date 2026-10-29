using Vaporlock.Application.DTOs.Noticias;

namespace Vaporlock.Application.Interfaces.Services;

public interface INoticiasService
{
    Task<IEnumerable<NoticiaDto>> ObtenerPublicadasAsync();
    Task<NoticiaDto?> ObtenerPorSlugAsync(string slug);
}