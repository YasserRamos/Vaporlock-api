using System.Text.Json;
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
                Orden,
                Publicado
            FROM dbo.MiembrosEquipo
            WHERE Publicado = 1
            ORDER BY Orden ASC, PkMiembroEquipo ASC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<MiembroEquipoDto>(sql);
    }

    public async Task<IEnumerable<MiembroEquipoDto>> ObtenerTodosAsync()
    {
        const string sql = """
            SELECT
                PkMiembroEquipo,
                FkMediaPortada,
                Nombre,
                Rol,
                Biografia,
                Enlace,
                Orden,
                Publicado
            FROM dbo.MiembrosEquipo
            ORDER BY Orden ASC, PkMiembroEquipo ASC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<MiembroEquipoDto>(sql);
    }

    public async Task<MiembroEquipoDto?> ObtenerPorIdAsync(int id)
    {
        const string sql = """
            SELECT
                PkMiembroEquipo,
                FkMediaPortada,
                Nombre,
                Rol,
                Biografia,
                Enlace,
                Orden,
                Publicado
            FROM dbo.MiembrosEquipo
            WHERE PkMiembroEquipo = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<MiembroEquipoDto>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> CrearAsync(CrearMiembroEquipoDto dto)
    {
        const string sql = """
            INSERT INTO dbo.MiembrosEquipo
            (
                FkMediaPortada,
                Nombre,
                Rol,
                Biografia,
                Enlace,
                Orden,
                Publicado
            )
            OUTPUT INSERTED.PkMiembroEquipo
            VALUES
            (
                @FkMediaPortada,
                @Nombre,
                @Rol,
                @Biografia,
                @Enlace,
                @Orden,
                @Publicado
            );
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleAsync<int>(sql, dto);
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ActualizarMiembroEquipoDto dto)
    {
        const string sql = """
            UPDATE dbo.MiembrosEquipo
            SET
                FkMediaPortada = @FkMediaPortada,
                Nombre = @Nombre,
                Rol = @Rol,
                Biografia = @Biografia,
                Enlace = @Enlace,
                Orden = @Orden,
                Publicado = @Publicado
            WHERE PkMiembroEquipo = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                dto.FkMediaPortada,
                dto.Nombre,
                dto.Rol,
                dto.Biografia,
                dto.Enlace,
                dto.Orden,
                dto.Publicado
            }
        );

        return filasAfectadas > 0;
    }

    public async Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarMiembroEquipoParcialDto dto)
    {
        var campos = new List<string>();
        var parametros = new DynamicParameters();

        parametros.Add("Id", id);

        if (dto.Nombre.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Nombre.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(dto.Nombre.GetString()))
            {
                throw new ArgumentException("El nombre no es válido.");
            }

            campos.Add("Nombre = @Nombre");
            parametros.Add("Nombre", dto.Nombre.GetString()!.Trim());
        }

        if (dto.Rol.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Rol.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(dto.Rol.GetString()))
            {
                throw new ArgumentException("El rol no es válido.");
            }

            campos.Add("Rol = @Rol");
            parametros.Add("Rol", dto.Rol.GetString()!.Trim());
        }

        if (dto.FkMediaPortada.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.FkMediaPortada.ValueKind != JsonValueKind.Null &&
                (dto.FkMediaPortada.ValueKind != JsonValueKind.Number ||
                 !dto.FkMediaPortada.TryGetInt32(out int portada) ||
                 portada <= 0))
            {
                throw new ArgumentException("El identificador de portada no es válido.");
            }

            int? portadaId = dto.FkMediaPortada.ValueKind == JsonValueKind.Null
                ? null
                : dto.FkMediaPortada.GetInt32();

            campos.Add("FkMediaPortada = @FkMediaPortada");
            parametros.Add("FkMediaPortada", portadaId);
        }

        if (dto.Biografia.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Biografia.ValueKind != JsonValueKind.String &&
                dto.Biografia.ValueKind != JsonValueKind.Null)
            {
                throw new ArgumentException("La biografía no es válida.");
            }

            campos.Add("Biografia = @Biografia");
            parametros.Add(
                "Biografia",
                dto.Biografia.ValueKind == JsonValueKind.Null
                    ? null
                    : dto.Biografia.GetString()
            );
        }

        if (dto.Enlace.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Enlace.ValueKind != JsonValueKind.String &&
                dto.Enlace.ValueKind != JsonValueKind.Null)
            {
                throw new ArgumentException("El enlace no es válido.");
            }

            campos.Add("Enlace = @Enlace");
            parametros.Add(
                "Enlace",
                dto.Enlace.ValueKind == JsonValueKind.Null
                    ? null
                    : dto.Enlace.GetString()
            );
        }

        if (dto.Orden.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Orden.ValueKind != JsonValueKind.Number ||
                !dto.Orden.TryGetInt32(out int orden) ||
                orden <= 0)
            {
                throw new ArgumentException("El orden debe ser un número mayor que cero.");
            }

            campos.Add("Orden = @Orden");
            parametros.Add("Orden", orden);
        }

        if (dto.Publicado.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Publicado.ValueKind != JsonValueKind.True &&
                dto.Publicado.ValueKind != JsonValueKind.False)
            {
                throw new ArgumentException("El estado de publicación no es válido.");
            }

            campos.Add("Publicado = @Publicado");
            parametros.Add("Publicado", dto.Publicado.GetBoolean());
        }

        if (campos.Count == 0)
        {
            throw new ArgumentException(
                "Debe proporcionar al menos un campo para actualizar."
            );
        }

        string sql = $"""
            UPDATE dbo.MiembrosEquipo
            SET {string.Join(", ", campos)}
            WHERE PkMiembroEquipo = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(sql, parametros);

        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        const string sql = """
            DELETE FROM dbo.MiembrosEquipo
            WHERE PkMiembroEquipo = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return filasAfectadas > 0;
    }
}