using System.Text.Json;
using Dapper;
using Vaporlock.Application.DTOs.Noticias;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Infrastructure.Repositories;

public class NoticiasRepository : INoticiasRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public NoticiasRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<NoticiaDto>> ObtenerPublicadasAsync()
    {
        const string sql = """
            SELECT
                PkNoticia,
                FkMediaPortada,
                Titulo,
                Slug,
                Resumen,
                Contenido,
                FechaCreacion,
                FechaPublicacion,
                Publicado
            FROM dbo.Noticias
            WHERE Publicado = 1
              AND (
                    FechaPublicacion IS NULL
                    OR FechaPublicacion <= SYSUTCDATETIME()
                  )
            ORDER BY FechaPublicacion DESC, FechaCreacion DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<NoticiaDto>(sql);
    }

    public async Task<NoticiaDto?> ObtenerPorSlugAsync(string slug)
    {
        const string sql = """
            SELECT
                PkNoticia,
                FkMediaPortada,
                Titulo,
                Slug,
                Resumen,
                Contenido,
                FechaCreacion,
                FechaPublicacion,
                Publicado
            FROM dbo.Noticias
            WHERE Slug = @Slug
              AND Publicado = 1
              AND (
                    FechaPublicacion IS NULL
                    OR FechaPublicacion <= SYSUTCDATETIME()
                  );
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<NoticiaDto>(
            sql,
            new { Slug = slug }
        );
    }

    public async Task<IEnumerable<NoticiaDto>> ObtenerTodasAsync()
    {
        const string sql = """
            SELECT
                PkNoticia,
                FkMediaPortada,
                Titulo,
                Slug,
                Resumen,
                Contenido,
                FechaCreacion,
                FechaPublicacion,
                Publicado
            FROM dbo.Noticias
            ORDER BY FechaCreacion DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<NoticiaDto>(sql);
    }

    public async Task<NoticiaDto?> ObtenerPorIdAsync(int id)
    {
        const string sql = """
            SELECT
                PkNoticia,
                FkMediaPortada,
                Titulo,
                Slug,
                Resumen,
                Contenido,
                FechaCreacion,
                FechaPublicacion,
                Publicado
            FROM dbo.Noticias
            WHERE PkNoticia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<NoticiaDto>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> CrearAsync(CrearNoticiaDto dto)
    {
        const string sql = """
            INSERT INTO dbo.Noticias
            (
                FkMediaPortada,
                Titulo,
                Slug,
                Resumen,
                Contenido,
                FechaCreacion,
                FechaPublicacion,
                Publicado
            )
            OUTPUT INSERTED.PkNoticia
            VALUES
            (
                @FkMediaPortada,
                @Titulo,
                @Slug,
                @Resumen,
                @Contenido,
                SYSUTCDATETIME(),
                @FechaPublicacion,
                @Publicado
            );
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleAsync<int>(sql, dto);
    }

    public async Task<bool> ActualizarAsync(
        int id,
        ActualizarNoticiaDto dto)
    {
        const string sql = """
            UPDATE dbo.Noticias
            SET
                FkMediaPortada = @FkMediaPortada,
                Titulo = @Titulo,
                Slug = @Slug,
                Resumen = @Resumen,
                Contenido = @Contenido,
                FechaPublicacion = @FechaPublicacion,
                Publicado = @Publicado
            WHERE PkNoticia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                dto.FkMediaPortada,
                dto.Titulo,
                dto.Slug,
                dto.Resumen,
                dto.Contenido,
                dto.FechaPublicacion,
                dto.Publicado
            }
        );

        return filasAfectadas > 0;
    }

    public async Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarNoticiaParcialDto dto)
    {
        var campos = new List<string>();
        var parametros = new DynamicParameters();

        parametros.Add("Id", id);

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

        if (dto.Titulo.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Titulo.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(dto.Titulo.GetString()))
            {
                throw new ArgumentException("El título no es válido.");
            }

            campos.Add("Titulo = @Titulo");
            parametros.Add("Titulo", dto.Titulo.GetString()!.Trim());
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

        if (dto.Resumen.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Resumen.ValueKind != JsonValueKind.String &&
                dto.Resumen.ValueKind != JsonValueKind.Null)
            {
                throw new ArgumentException("El resumen no es válido.");
            }

            campos.Add("Resumen = @Resumen");
            parametros.Add(
                "Resumen",
                dto.Resumen.ValueKind == JsonValueKind.Null
                    ? null
                    : dto.Resumen.GetString()
            );
        }

        if (dto.Contenido.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Contenido.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(dto.Contenido.GetString()))
            {
                throw new ArgumentException("El contenido no es válido.");
            }

            campos.Add("Contenido = @Contenido");
            parametros.Add("Contenido", dto.Contenido.GetString());
        }

        if (dto.FechaPublicacion.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.FechaPublicacion.ValueKind != JsonValueKind.Null &&
                (dto.FechaPublicacion.ValueKind != JsonValueKind.String ||
                 !dto.FechaPublicacion.TryGetDateTime(out DateTime fecha)))
            {
                throw new ArgumentException(
                    "La fecha de publicación debe tener un formato de fecha válido."
                );
            }

            DateTime? fechaPublicacion =
                dto.FechaPublicacion.ValueKind == JsonValueKind.Null
                    ? null
                    : dto.FechaPublicacion.GetDateTime();

            campos.Add("FechaPublicacion = @FechaPublicacion");
            parametros.Add("FechaPublicacion", fechaPublicacion);
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
            UPDATE dbo.Noticias
            SET {string.Join(", ", campos)}
            WHERE PkNoticia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(sql, parametros);

        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        const string sql = """
            DELETE FROM dbo.Noticias
            WHERE PkNoticia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        int filasAfectadas = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return filasAfectadas > 0;
    }
}