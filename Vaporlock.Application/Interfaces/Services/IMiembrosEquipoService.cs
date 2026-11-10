using Vaporlock.Application.DTOs.MiembrosEquipo;

namespace Vaporlock.Application.Interfaces.Services;

public interface IMiembrosEquipoService
{
    // Consultas públicas
    Task<IEnumerable<MiembroEquipoDto>> ObtenerPublicadosAsync();

    // Consultas administrativas
    Task<IEnumerable<MiembroEquipoDto>> ObtenerTodosAsync();
    Task<MiembroEquipoDto?> ObtenerPorIdAsync(int id);

    // Operaciones CRUD
    Task<int> CrearAsync(CrearMiembroEquipoDto dto);
    Task<bool> ActualizarAsync(int id, ActualizarMiembroEquipoDto dto);
    Task<bool> ActualizarParcialAsync(int id, ActualizarMiembroEquipoParcialDto dto);
    Task<bool> EliminarAsync(int id);
}