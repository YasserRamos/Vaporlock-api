using Dapper;
using Vaporlock.Application.DTOs.MiembrosEquipo;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Infrastructure.Repositories;

public class MiembrosEquipoRepository : IMiembrosEquipoRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public MiembrosEquipoRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<MiembroEquipoDto>> ObtenerPublicadosAsync()
    {
        const string sql = """
            SELECT
                PkMiembroEquipo,
                FkMediaPortada,
                Nombre,
                Rol,
                Biografia,
                Enlace,
                Orden
            FROM dbo.MiembrosEquipo
            WHERE Publicado = 1
            ORDER BY Orden ASC, PkMiembroEquipo ASC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<MiembroEquipoDto>(sql);
    }
}