using System.Text.Json;
using Dapper;
using Vaporlock.Application.DTOs.Niveles;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Infrastructure.Repositories;

public class NivelesRepository : INivelesRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public NivelesRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<NivelDto>> ObtenerPublicadosAsync()
    {
        const string sql = """
            SELECT
                PkNivel,
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Orden,
                Publicado
            FROM dbo.Niveles
            WHERE Publicado = 1
            ORDER BY Orden ASC, PkNivel ASC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<NivelDto>(sql);
    }

    public async Task<NivelDto?> ObtenerPorSlugAsync(string slug)
    {
        const string sql = """
            SELECT
                PkNivel,
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Orden,
                Publicado
            FROM dbo.Niveles
            WHERE Slug = @Slug
              AND Publicado = 1;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<NivelDto>(
            sql,
            new { Slug = slug }
        );
    }

    public async Task<IEnumerable<NivelDto>> ObtenerTodosAsync()
    {
        const string sql = """
            SELECT
                PkNivel,
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Orden,
                Publicado
            FROM dbo.Niveles
            ORDER BY Orden ASC, PkNivel ASC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<NivelDto>(sql);
    }

    public async Task<NivelDto?> ObtenerPorIdAsync(int id)
    {
        const string sql = """
            SELECT
                PkNivel,
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Orden,
                Publicado
            FROM dbo.Niveles
            WHERE PkNivel = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<NivelDto>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> CrearAsync(CrearNivelDto dto)
    {
        const string sql = """
            INSERT INTO dbo.Niveles
            (
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Orden,
                Publicado
            )
            OUTPUT INSERTED.PkNivel
            VALUES
            (
                @FkMediaPortada,
                @Nombre,
                @Slug,
                @Descripcion,
                @Orden,
                @Publicado
            );
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleAsync<int>(sql, dto);
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ActualizarNivelDto dto)
    {
        const string sql = """
            UPDATE dbo.Niveles
            SET
                FkMediaPortada = @FkMediaPortada,
                Nombre = @Nombre,
                Slug = @Slug,
                Descripcion = @Descripcion,
                Orden = @Orden,
                Publicado = @Publicado
            WHERE PkNivel = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                dto.FkMediaPortada,
                dto.Nombre,
                dto.Slug,
                dto.Descripcion,
                dto.Orden,
                dto.Publicado
            }
        );

        return filasAfectadas > 0;
    }

    public async Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarNivelParcialDto dto)
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

        if (dto.Slug.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Slug.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(dto.Slug.GetString()))
            {
                throw new ArgumentException("El slug no es válido.");
            }

            campos.Add("Slug = @Slug");
            parametros.Add(
                "Slug",
                dto.Slug.GetString()!.Trim().ToLowerInvariant()
            );
        }

        if (dto.FkMediaPortada.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.FkMediaPortada.ValueKind != JsonValueKind.Null &&
                (dto.FkMediaPortada.ValueKind != JsonValueKind.Number ||
                 !dto.FkMediaPortada.TryGetInt32(out int portada) ||
                 portada <= 0))
            {
                throw new ArgumentException(
                    "El identificador de portada debe ser mayor que cero o null."
                );
            }

            int? portadaId = dto.FkMediaPortada.ValueKind == JsonValueKind.Null
                ? null
                : dto.FkMediaPortada.GetInt32();

            campos.Add("FkMediaPortada = @FkMediaPortada");
            parametros.Add("FkMediaPortada", portadaId);
        }

        if (dto.Descripcion.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Descripcion.ValueKind != JsonValueKind.String &&
                dto.Descripcion.ValueKind != JsonValueKind.Null)
            {
                throw new ArgumentException("La descripción no es válida.");
            }

            campos.Add("Descripcion = @Descripcion");
            parametros.Add(
                "Descripcion",
                dto.Descripcion.ValueKind == JsonValueKind.Null
                    ? null
                    : dto.Descripcion.GetString()
            );
        }

        if (dto.Orden.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Orden.ValueKind != JsonValueKind.Number ||
                !dto.Orden.TryGetInt32(out int orden) ||
                orden <= 0)
            {
                throw new ArgumentException(
                    "El orden debe ser un número mayor que cero."
                );
            }

            campos.Add("Orden = @Orden");
            parametros.Add("Orden", orden);
        }

        if (dto.Publicado.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Publicado.ValueKind != JsonValueKind.True &&
                dto.Publicado.ValueKind != JsonValueKind.False)
            {
                throw new ArgumentException(
                    "El estado de publicación debe ser true o false."
                );
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
            UPDATE dbo.Niveles
            SET {string.Join(", ", campos)}
            WHERE PkNivel = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(sql, parametros);

        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        const string sql = """
            DELETE FROM dbo.Niveles
            WHERE PkNivel = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return filasAfectadas > 0;
    }
}