
using System.Text.Json;
using Dapper;
using Vaporlock.Application.DTOs.Personajes;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Infrastructure.Repositories;

public class PersonajesRepository : IPersonajesRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public PersonajesRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // CONSULTAS

    public Task<IEnumerable<PersonajeDto>> ObtenerPublicadosAsync()
    {
        return ObtenerAsync(soloPublicados: true);
    }

    public async Task<PersonajeDto?> ObtenerPorSlugAsync(string slug)
    {
        var personajes = await ObtenerAsync(
            slug: slug,
            soloPublicados: true
        );

        return personajes.FirstOrDefault();
    }

    public Task<IEnumerable<PersonajeDto>> ObtenerTodosAsync()
    {
        return ObtenerAsync(soloPublicados: false);
    }

    public async Task<PersonajeDto?> ObtenerPorIdAsync(int id)
    {
        var personajes = await ObtenerAsync(
            id: id,
            soloPublicados: false
        );

        return personajes.FirstOrDefault();
    }

    private async Task<IEnumerable<PersonajeDto>> ObtenerAsync(
        string? slug = null,
        int? id = null,
        bool soloPublicados = true)
    {
        const string sql = """
            SELECT
                P.PkPersonaje,
                P.FkMediaPortada,
                CASE
                    WHEN P.FkMediaPortada IS NOT NULL
                    THEN CONCAT('/api/media/', P.FkMediaPortada)
                    ELSE NULL
                END AS UrlRetrato,
                P.Nombre,
                P.Slug,
                P.Descripcion,
                P.Historia,
                P.Rol,
                P.PuntosSalud,
                P.Agresividad,
                P.Publicado
            FROM dbo.Personajes P
            WHERE (@SoloPublicados = 0 OR P.Publicado = 1)
              AND (@Slug IS NULL OR P.Slug = @Slug)
              AND (@Id IS NULL OR P.PkPersonaje = @Id)
            ORDER BY P.PkPersonaje;

            SELECT
                A.FkPersonaje AS PkPersonaje,
                A.Nombre AS Valor
            FROM dbo.PersonajesApariciones A
            INNER JOIN dbo.Personajes P
                ON P.PkPersonaje = A.FkPersonaje
            WHERE (@SoloPublicados = 0 OR P.Publicado = 1)
              AND (@Slug IS NULL OR P.Slug = @Slug)
              AND (@Id IS NULL OR P.PkPersonaje = @Id);

            SELECT
                A.FkPersonaje AS PkPersonaje,
                A.Nombre AS Valor
            FROM dbo.PersonajesAcciones A
            INNER JOIN dbo.Personajes P
                ON P.PkPersonaje = A.FkPersonaje
            WHERE (@SoloPublicados = 0 OR P.Publicado = 1)
              AND (@Slug IS NULL OR P.Slug = @Slug)
              AND (@Id IS NULL OR P.PkPersonaje = @Id);

            SELECT
                H.FkPersonaje AS PkPersonaje,
                H.Nombre AS Valor
            FROM dbo.PersonajesHabilidades H
            INNER JOIN dbo.Personajes P
                ON P.PkPersonaje = H.FkPersonaje
            WHERE (@SoloPublicados = 0 OR P.Publicado = 1)
              AND (@Slug IS NULL OR P.Slug = @Slug)
              AND (@Id IS NULL OR P.PkPersonaje = @Id);
            """;

        using var connection = _connectionFactory.CreateConnection();

        using var results = await connection.QueryMultipleAsync(
            sql,
            new
            {
                Slug = slug,
                Id = id,
                SoloPublicados = soloPublicados
            }
        );

        var personajes = (await results.ReadAsync<PersonajeDto>()).ToList();

        var apariciones = await results.ReadAsync<RelacionDto>();
        var acciones = await results.ReadAsync<RelacionDto>();
        var habilidades = await results.ReadAsync<RelacionDto>();

        var personajesPorId = personajes.ToDictionary(
            personaje => personaje.PkPersonaje
        );

        foreach (var item in apariciones)
        {
            if (personajesPorId.TryGetValue(
                item.PkPersonaje,
                out var personaje))
            {
                personaje.Apariciones.Add(item.Valor);
            }
        }

        foreach (var item in acciones)
        {
            if (personajesPorId.TryGetValue(
                item.PkPersonaje,
                out var personaje))
            {
                personaje.Acciones.Add(item.Valor);
            }
        }

        foreach (var item in habilidades)
        {
            if (personajesPorId.TryGetValue(
                item.PkPersonaje,
                out var personaje))
            {
                personaje.Habilidades.Add(item.Valor);
            }
        }

        return personajes;
    }

    // CREAR

    public async Task<int> CrearAsync(CrearPersonajeDto dto)
    {
        const string sql = """
            INSERT INTO dbo.Personajes
            (
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Historia,
                Rol,
                PuntosSalud,
                Agresividad,
                Publicado
            )
            OUTPUT INSERTED.PkPersonaje
            VALUES
            (
                @FkMediaPortada,
                @Nombre,
                @Slug,
                @Descripcion,
                @Historia,
                @Rol,
                @PuntosSalud,
                @Agresividad,
                @Publicado
            );
            """;

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var id = await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    dto.FkMediaPortada,
                    dto.Nombre,
                    dto.Slug,
                    dto.Descripcion,
                    dto.Historia,
                    dto.Rol,
                    dto.PuntosSalud,
                    dto.Agresividad,
                    dto.Publicado
                },
                transaction
            );

            await ReemplazarRelacionesAsync(
                connection,
                transaction,
                id,
                dto.Apariciones,
                dto.Acciones,
                dto.Habilidades
            );

            transaction.Commit();

            return id;
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
        ActualizarPersonajeDto dto)
    {
        const string sql = """
            UPDATE dbo.Personajes
            SET
                FkMediaPortada = @FkMediaPortada,
                Nombre = @Nombre,
                Slug = @Slug,
                Descripcion = @Descripcion,
                Historia = @Historia,
                Rol = @Rol,
                PuntosSalud = @PuntosSalud,
                Agresividad = @Agresividad,
                Publicado = @Publicado
            WHERE PkPersonaje = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var filas = await connection.ExecuteAsync(
                sql,
                new
                {
                    Id = id,
                    dto.FkMediaPortada,
                    dto.Nombre,
                    dto.Slug,
                    dto.Descripcion,
                    dto.Historia,
                    dto.Rol,
                    dto.PuntosSalud,
                    dto.Agresividad,
                    dto.Publicado
                },
                transaction
            );

            if (filas == 0)
            {
                transaction.Rollback();
                return false;
            }

            await ReemplazarRelacionesAsync(
                connection,
                transaction,
                id,
                dto.Apariciones,
                dto.Acciones,
                dto.Habilidades
            );

            transaction.Commit();

            return true;
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
        ActualizarPersonajeParcialDto dto)
    {
        var asignaciones = new List<string>();
        var parametros = new DynamicParameters();

        void Agregar(string columna, string parametro, object? valor)
        {
            asignaciones.Add($"{columna} = @{parametro}");
            parametros.Add(parametro, valor);
        }

        if (dto.FkMediaPortada.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "FkMediaPortada",
                "FkMediaPortada",
                LeerEnteroNullable(dto.FkMediaPortada, "FkMediaPortada")
            );
        }

        if (dto.Nombre.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "Nombre",
                "Nombre",
                LeerTextoObligatorio(dto.Nombre, "Nombre")
            );
        }

        if (dto.Slug.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "Slug",
                "Slug",
                LeerTextoObligatorio(dto.Slug, "Slug")
            );
        }

        if (dto.Descripcion.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "Descripcion",
                "Descripcion",
                LeerTextoNullable(dto.Descripcion, "Descripcion")
            );
        }

        if (dto.Historia.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "Historia",
                "Historia",
                LeerTextoNullable(dto.Historia, "Historia")
            );
        }

        if (dto.Rol.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "Rol",
                "Rol",
                LeerTextoObligatorio(dto.Rol, "Rol")
            );
        }

        if (dto.PuntosSalud.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "PuntosSalud",
                "PuntosSalud",
                LeerEnteroNullable(dto.PuntosSalud, "PuntosSalud")
            );
        }

        if (dto.Agresividad.ValueKind != JsonValueKind.Undefined)
        {
            Agregar(
                "Agresividad",
                "Agresividad",
                LeerTextoNullable(dto.Agresividad, "Agresividad")
            );
        }

        if (dto.Publicado.ValueKind != JsonValueKind.Undefined)
        {
            if (dto.Publicado.ValueKind is not JsonValueKind.True
                and not JsonValueKind.False)
            {
                throw new ArgumentException(
                    "Publicado debe ser un booleano."
                );
            }

            Agregar(
                "Publicado",
                "Publicado",
                dto.Publicado.GetBoolean()
            );
        }

        var apariciones = dto.Apariciones.ValueKind != JsonValueKind.Undefined
            ? LeerLista(dto.Apariciones, "Apariciones")
            : null;

        var acciones = dto.Acciones.ValueKind != JsonValueKind.Undefined
            ? LeerLista(dto.Acciones, "Acciones")
            : null;

        var habilidades = dto.Habilidades.ValueKind != JsonValueKind.Undefined
            ? LeerLista(dto.Habilidades, "Habilidades")
            : null;

        if (asignaciones.Count == 0 &&
            apariciones is null &&
            acciones is null &&
            habilidades is null)
        {
            throw new ArgumentException(
                "Debes proporcionar al menos un campo para actualizar."
            );
        }

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            if (asignaciones.Count > 0)
            {
                parametros.Add("Id", id);

                var sql = $"""
                    UPDATE dbo.Personajes
                    SET {string.Join(", ", asignaciones)}
                    WHERE PkPersonaje = @Id;
                    """;

                var filas = await connection.ExecuteAsync(
                    sql,
                    parametros,
                    transaction
                );

                if (filas == 0)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            else
            {
                var existe = await connection.ExecuteScalarAsync<int>(
                    """
                    SELECT COUNT(1)
                    FROM dbo.Personajes
                    WHERE PkPersonaje = @Id;
                    """,
                    new { Id = id },
                    transaction
                );

                if (existe == 0)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            if (apariciones is not null)
            {
                await ReemplazarListaAsync(
                    connection,
                    transaction,
                    id,
                    "dbo.PersonajesApariciones",
                    apariciones
                );
            }

            if (acciones is not null)
            {
                await ReemplazarListaAsync(
                    connection,
                    transaction,
                    id,
                    "dbo.PersonajesAcciones",
                    acciones
                );
            }

            if (habilidades is not null)
            {
                await ReemplazarListaAsync(
                    connection,
                    transaction,
                    id,
                    "dbo.PersonajesHabilidades",
                    habilidades
                );
            }

            transaction.Commit();

            return true;
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
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var existe = await connection.ExecuteScalarAsync<int>(
                """
                SELECT COUNT(1)
                FROM dbo.Personajes WITH (UPDLOCK, HOLDLOCK)
                WHERE PkPersonaje = @Id;
                """,
                new { Id = id },
                transaction
            );

            if (existe == 0)
            {
                transaction.Rollback();
                return false;
            }

            await connection.ExecuteAsync(
                """
                DELETE FROM dbo.PersonajesApariciones
                WHERE FkPersonaje = @Id;

                DELETE FROM dbo.PersonajesAcciones
                WHERE FkPersonaje = @Id;

                DELETE FROM dbo.PersonajesHabilidades
                WHERE FkPersonaje = @Id;
                """,
                new { Id = id },
                transaction
            );

            await connection.ExecuteAsync(
                """
                DELETE FROM dbo.Personajes
                WHERE PkPersonaje = @Id;
                """,
                new { Id = id },
                transaction
            );

            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // GESTIÓN DE RELACIONES

    private static async Task ReemplazarRelacionesAsync(
        System.Data.IDbConnection connection,
        System.Data.IDbTransaction transaction,
        int id,
        IEnumerable<string>? apariciones,
        IEnumerable<string>? acciones,
        IEnumerable<string>? habilidades)
    {
        await ReemplazarListaAsync(
            connection,
            transaction,
            id,
            "dbo.PersonajesApariciones",
            apariciones
        );

        await ReemplazarListaAsync(
            connection,
            transaction,
            id,
            "dbo.PersonajesAcciones",
            acciones
        );

        await ReemplazarListaAsync(
            connection,
            transaction,
            id,
            "dbo.PersonajesHabilidades",
            habilidades
        );
    }

    private static async Task ReemplazarListaAsync(
        System.Data.IDbConnection connection,
        System.Data.IDbTransaction transaction,
        int id,
        string tabla,
        IEnumerable<string>? valores)
    {
        var sqlEliminar = $"""
            DELETE FROM {tabla}
            WHERE FkPersonaje = @Id;
            """;

        await connection.ExecuteAsync(
            sqlEliminar,
            new { Id = id },
            transaction
        );

        var elementos = (valores ?? [])
            .Where(valor => !string.IsNullOrWhiteSpace(valor))
            .Select(valor => new
            {
                FkPersonaje = id,
                Nombre = valor.Trim()
            })
            .ToList();

        if (elementos.Count == 0)
            return;

        var sqlInsertar = $"""
            INSERT INTO {tabla} (FkPersonaje, Nombre)
            VALUES (@FkPersonaje, @Nombre);
            """;

        await connection.ExecuteAsync(
            sqlInsertar,
            elementos,
            transaction
        );
    }

    // CONVERSIÓN DE VALORES PATCH

    private static string LeerTextoObligatorio(
        JsonElement elemento,
        string campo)
    {
        if (elemento.ValueKind != JsonValueKind.String)
        {
            throw new ArgumentException(
                $"{campo} debe ser un texto."
            );
        }

        var valor = elemento.GetString();

        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException(
                $"{campo} es obligatorio."
            );
        }

        return valor.Trim();
    }

    private static string? LeerTextoNullable(
        JsonElement elemento,
        string campo)
    {
        if (elemento.ValueKind == JsonValueKind.Null)
            return null;

        if (elemento.ValueKind != JsonValueKind.String)
        {
            throw new ArgumentException(
                $"{campo} debe ser un texto o null."
            );
        }

        return elemento.GetString();
    }

    private static int? LeerEnteroNullable(
        JsonElement elemento,
        string campo)
    {
        if (elemento.ValueKind == JsonValueKind.Null)
            return null;

        if (elemento.ValueKind != JsonValueKind.Number ||
            !elemento.TryGetInt32(out var valor))
        {
            throw new ArgumentException(
                $"{campo} debe ser un entero o null."
            );
        }

        return valor;
    }

    private static List<string> LeerLista(
        JsonElement elemento,
        string campo)
    {
        if (elemento.ValueKind == JsonValueKind.Null)
            return [];

        if (elemento.ValueKind != JsonValueKind.Array)
        {
            throw new ArgumentException(
                $"{campo} debe ser un arreglo."
            );
        }

        var resultado = new List<string>();

        foreach (var item in elemento.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException(
                    $"Todos los elementos de {campo} deben ser textos."
                );
            }

            var valor = item.GetString();

            if (!string.IsNullOrWhiteSpace(valor))
                resultado.Add(valor.Trim());
        }

        return resultado;
    }

    private sealed class RelacionDto
    {
        public int PkPersonaje { get; set; }
        public string Valor { get; set; } = string.Empty;
    }
}
