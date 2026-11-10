
using System.Text.Json;
using Dapper;
using Microsoft.Data.SqlClient;
using Vaporlock.Application.DTOs.Historias;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Infrastructure.Repositories;

public class HistoriasRepository : IHistoriasRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public HistoriasRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // CONSULTAS

    public async Task<IEnumerable<HistoriaDto>> ObtenerPublicadasAsync()
    {
        const string sql = """
            SELECT
                PkHistoria,
                FkMediaPortada,
                Titulo,
                Slug,
                Contenido,
                Orden,
                Publicado
            FROM dbo.Historias
            WHERE Publicado = 1
            ORDER BY Orden ASC, PkHistoria ASC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<HistoriaDto>(sql);
    }

    public async Task<HistoriaDto?> ObtenerPorSlugAsync(string slug)
    {
        const string sql = """
            SELECT
                PkHistoria,
                FkMediaPortada,
                Titulo,
                Slug,
                Contenido,
                Orden,
                Publicado
            FROM dbo.Historias
            WHERE Slug = @Slug
              AND Publicado = 1;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<HistoriaDto>(
            sql,
            new { Slug = slug }
        );
    }

    public async Task<IEnumerable<HistoriaDto>> ObtenerTodasAsync()
    {
        const string sql = """
            SELECT
                PkHistoria,
                FkMediaPortada,
                Titulo,
                Slug,
                Contenido,
                Orden,
                Publicado
            FROM dbo.Historias
            ORDER BY Orden ASC, PkHistoria ASC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<HistoriaDto>(sql);
    }

    public async Task<HistoriaDto?> ObtenerPorIdAsync(int id)
    {
        const string sql = """
            SELECT
                PkHistoria,
                FkMediaPortada,
                Titulo,
                Slug,
                Contenido,
                Orden,
                Publicado
            FROM dbo.Historias
            WHERE PkHistoria = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<HistoriaDto>(
            sql,
            new { Id = id }
        );
    }

    // CREAR

    public async Task<int> CrearAsync(CrearHistoriaDto dto)
    {
        const string sql = """
            INSERT INTO dbo.Historias
            (
                FkMediaPortada,
                Titulo,
                Slug,
                Contenido,
                Orden,
                Publicado
            )
            OUTPUT INSERTED.PkHistoria
            VALUES
            (
                @FkMediaPortada,
                @Titulo,
                @Slug,
                @Contenido,
                @Orden,
                @Publicado
            );
            """;

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            await ValidarMediaAsync(
                connection,
                transaction,
                dto.FkMediaPortada
            );

            await ValidarSlugDisponibleAsync(
                connection,
                transaction,
                dto.Slug
            );

            var id = await connection.QuerySingleAsync<int>(
                sql,
                dto,
                transaction
            );

            transaction.Commit();

            return id;
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw TraducirErrorSql(ex);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // ACTUALIZAR COMPLETAMENTE

    public async Task<bool> ActualizarAsync(
        int id,
        ActualizarHistoriaDto dto)
    {
        const string sql = """
            UPDATE dbo.Historias
            SET
                FkMediaPortada = @FkMediaPortada,
                Titulo = @Titulo,
                Slug = @Slug,
                Contenido = @Contenido,
                Orden = @Orden,
                Publicado = @Publicado
            WHERE PkHistoria = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var existe = await ExisteHistoriaAsync(
                connection,
                transaction,
                id
            );

            if (!existe)
            {
                transaction.Rollback();
                return false;
            }

            await ValidarMediaAsync(
                connection,
                transaction,
                dto.FkMediaPortada
            );

            await ValidarSlugDisponibleAsync(
                connection,
                transaction,
                dto.Slug,
                id
            );

            var filasAfectadas = await connection.ExecuteAsync(
                sql,
                new
                {
                    Id = id,
                    dto.FkMediaPortada,
                    dto.Titulo,
                    dto.Slug,
                    dto.Contenido,
                    dto.Orden,
                    dto.Publicado
                },
                transaction
            );

            transaction.Commit();

            return filasAfectadas > 0;
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw TraducirErrorSql(ex);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // ACTUALIZAR PARCIALMENTE

    public async Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarHistoriaParcialDto dto)
    {
        var campos = new List<string>();
        var parametros = new DynamicParameters();

        parametros.Add("Id", id);

        if (dto.Titulo.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Titulo.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(dto.Titulo.GetString()))
            {
                throw new ArgumentException(
                    "El título proporcionado no es válido."
                );
            }

            campos.Add("Titulo = @Titulo");
            parametros.Add("Titulo", dto.Titulo.GetString()!.Trim());
        }

        if (dto.Slug.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Slug.ValueKind != JsonValueKind.String ||
                string.IsNullOrWhiteSpace(dto.Slug.GetString()))
            {
                throw new ArgumentException(
                    "El slug proporcionado no es válido."
                );
            }

            var slug = dto.Slug.GetString()!.Trim().ToLowerInvariant();

            campos.Add("Slug = @Slug");
            parametros.Add("Slug", slug);
        }

        if (dto.Contenido.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Contenido.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException(
                    "El contenido proporcionado no es válido."
                );
            }

            campos.Add("Contenido = @Contenido");
            parametros.Add("Contenido", dto.Contenido.GetString());
        }

        int? fkMediaPortada = null;
        bool actualizarPortada =
            dto.FkMediaPortada.ValueKind != JsonValueKind.Undefined;

        if (actualizarPortada)
        {
            if (dto.FkMediaPortada.ValueKind == JsonValueKind.Null)
            {
                fkMediaPortada = null;
            }
            else if (
                dto.FkMediaPortada.ValueKind == JsonValueKind.Number &&
                dto.FkMediaPortada.TryGetInt32(out var portada) &&
                portada > 0)
            {
                fkMediaPortada = portada;
            }
            else
            {
                throw new ArgumentException(
                    "El identificador de portada debe ser un entero positivo o null."
                );
            }

            campos.Add("FkMediaPortada = @FkMediaPortada");
            parametros.Add("FkMediaPortada", fkMediaPortada);
        }

        if (dto.Orden.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Orden.ValueKind != JsonValueKind.Number ||
                !dto.Orden.TryGetInt32(out var orden) ||
                orden <= 0)
            {
                throw new ArgumentException(
                    "El orden debe ser un entero mayor que cero."
                );
            }

            campos.Add("Orden = @Orden");
            parametros.Add("Orden", orden);
        }

        if (dto.Publicado.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Publicado.ValueKind is not JsonValueKind.True
                and not JsonValueKind.False)
            {
                throw new ArgumentException(
                    "El estado de publicación debe ser un booleano."
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

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var existe = await ExisteHistoriaAsync(
                connection,
                transaction,
                id
            );

            if (!existe)
            {
                transaction.Rollback();
                return false;
            }

            if (actualizarPortada)
            {
                await ValidarMediaAsync(
                    connection,
                    transaction,
                    fkMediaPortada
                );
            }

            if (dto.Slug.ValueKind != JsonValueKind.Undefined)
            {
                await ValidarSlugDisponibleAsync(
                    connection,
                    transaction,
                    parametros.Get<string>("Slug"),
                    id
                );
            }

            var sql = $"""
                UPDATE dbo.Historias
                SET {string.Join(", ", campos)}
                WHERE PkHistoria = @Id;
                """;

            var filasAfectadas = await connection.ExecuteAsync(
                sql,
                parametros,
                transaction
            );

            transaction.Commit();

            return filasAfectadas > 0;
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw TraducirErrorSql(ex);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // ELIMINAR

    public async Task<bool> EliminarAsync(int id)
    {
        const string sql = """
            DELETE FROM dbo.Historias
            WHERE PkHistoria = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var filasAfectadas = await connection.ExecuteAsync(
                sql,
                new { Id = id },
                transaction
            );

            transaction.Commit();

            return filasAfectadas > 0;
        }
        catch (SqlException ex)
        {
            transaction.Rollback();
            throw TraducirErrorSql(ex);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // VALIDACIONES DE BASE DE DATOS

    private static async Task<bool> ExisteHistoriaAsync(
        System.Data.IDbConnection connection,
        System.Data.IDbTransaction transaction,
        int id)
    {
        return await connection.ExecuteScalarAsync<bool>(
            """
            SELECT CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM dbo.Historias
                    WHERE PkHistoria = @Id
                )
                THEN 1
                ELSE 0
            END;
            """,
            new { Id = id },
            transaction
        );
    }

    private static async Task ValidarMediaAsync(
        System.Data.IDbConnection connection,
        System.Data.IDbTransaction transaction,
        int? fkMediaPortada)
    {
        // La portada es opcional.
        if (!fkMediaPortada.HasValue)
            return;

        var existe = await connection.ExecuteScalarAsync<bool>(
            """
            SELECT CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM dbo.Media
                    WHERE PkMedia = @Id
                )
                THEN 1
                ELSE 0
            END;
            """,
            new { Id = fkMediaPortada.Value },
            transaction
        );

        if (!existe)
        {
            throw new ArgumentException(
                $"La media de portada con ID {fkMediaPortada.Value} no existe."
            );
        }
    }

    private static async Task ValidarSlugDisponibleAsync(
        System.Data.IDbConnection connection,
        System.Data.IDbTransaction transaction,
        string slug,
        int? idExcluir = null)
    {
        var existe = await connection.ExecuteScalarAsync<bool>(
            """
            SELECT CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM dbo.Historias
                    WHERE Slug = @Slug
                      AND (@IdExcluir IS NULL OR PkHistoria <> @IdExcluir)
                )
                THEN 1
                ELSE 0
            END;
            """,
            new
            {
                Slug = slug,
                IdExcluir = idExcluir
            },
            transaction
        );

        if (existe)
        {
            throw new ArgumentException(
                $"Ya existe una historia registrada con el slug '{slug}'."
            );
        }
    }

    // TRADUCCIÓN DE ERRORES SQL

    private static Exception TraducirErrorSql(SqlException ex)
    {
        return ex.Number switch
        {
            547 => new ArgumentException(
                "La operación no se puede completar porque existe una referencia relacionada inválida.",
                ex
            ),

            2601 or 2627 => new ArgumentException(
                "Ya existe un registro con un valor único duplicado. Comprueba el slug.",
                ex
            ),

            515 => new ArgumentException(
                "Falta proporcionar un campo obligatorio.",
                ex
            ),

            2628 or 8152 => new ArgumentException(
                "Uno de los valores supera la capacidad permitida por la base de datos.",
                ex
            ),

            _ => ex
        };
    }
}
