using Vaporlock.Application.DTOs.MiembrosEquipo;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class MiembrosEquipoService : IMiembrosEquipoService
{
    private readonly IMiembrosEquipoRepository _repository;

    public MiembrosEquipoService(IMiembrosEquipoRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<MiembroEquipoDto>> ObtenerPublicadosAsync()
    {
        return _repository.ObtenerPublicadosAsync();
    }
}