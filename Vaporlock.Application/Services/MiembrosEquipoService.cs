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

    public Task<IEnumerable<MiembroEquipoDto>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<MiembroEquipoDto?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task<int> CrearAsync(CrearMiembroEquipoDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        dto.Rol = dto.Rol.Trim();

        return _repository.CrearAsync(dto);
    }

    public Task<bool> ActualizarAsync(
        int id,
        ActualizarMiembroEquipoDto dto)
    {
        dto.Nombre = dto.Nombre.Trim();
        dto.Rol = dto.Rol.Trim();

        return _repository.ActualizarAsync(id, dto);
    }

    public Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarMiembroEquipoParcialDto dto)
    {
        return _repository.ActualizarParcialAsync(id, dto);
    }

    public Task<bool> EliminarAsync(int id)
    {
        return _repository.EliminarAsync(id);
    }
}