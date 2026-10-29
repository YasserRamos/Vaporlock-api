using Vaporlock.Application.DTOs.MiembrosEquipo;

namespace Vaporlock.Application.Interfaces.Repositories;

public interface IMiembrosEquipoRepository
{
    Task<IEnumerable<MiembroEquipoDto>> ObtenerPublicadosAsync();
}