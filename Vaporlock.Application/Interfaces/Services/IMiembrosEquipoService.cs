using Vaporlock.Application.DTOs.MiembrosEquipo;

namespace Vaporlock.Application.Interfaces.Services;

public interface IMiembrosEquipoService
{
    Task<IEnumerable<MiembroEquipoDto>> ObtenerPublicadosAsync();
}