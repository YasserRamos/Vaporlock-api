
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

    public async Task<IEnumerable<PersonajeDto>> ObtenerPublicadosAsync()
    {
        return await ObtenerAsync();
    }

    public async Task<PersonajeDto?> ObtenerPorSlugAsync(string slug)
    {
        var personajes = await ObtenerAsync(slug);

        return personajes.FirstOrDefault();
    }

    private async Task<List<PersonajeDto>> ObtenerAsync(string? slug = null)
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
                P.Agresividad
            FROM dbo.Personajes P
            WHERE P.Publicado = 1
              AND (@Slug IS NULL OR P.Slug = @Slug)
            ORDER BY P.PkPersonaje;

            SELECT
                A.FkPersonaje AS PkPersonaje,
                A.Nombre AS Valor
            FROM dbo.PersonajesApariciones A
            INNER JOIN dbo.Personajes P
                ON P.PkPersonaje = A.FkPersonaje
            WHERE P.Publicado = 1
              AND (@Slug IS NULL OR P.Slug = @Slug);

            SELECT
                A.FkPersonaje AS PkPersonaje,
                A.Nombre AS Valor
            FROM dbo.PersonajesAcciones A
            INNER JOIN dbo.Personajes P
                ON P.PkPersonaje = A.FkPersonaje
            WHERE P.Publicado = 1
              AND (@Slug IS NULL OR P.Slug = @Slug);

            SELECT
                H.FkPersonaje AS PkPersonaje,
                H.Nombre AS Valor
            FROM dbo.PersonajesHabilidades H
            INNER JOIN dbo.Personajes P
                ON P.PkPersonaje = H.FkPersonaje
            WHERE P.Publicado = 1
              AND (@Slug IS NULL OR P.Slug = @Slug);
            """;

        using var connection = _connectionFactory.CreateConnection();

        using var results = await connection.QueryMultipleAsync(
            sql,
            new { Slug = slug }
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
            if (personajesPorId.TryGetValue(item.PkPersonaje, out var personaje))
                personaje.Apariciones.Add(item.Valor);
        }

        foreach (var item in acciones)
        {
            if (personajesPorId.TryGetValue(item.PkPersonaje, out var personaje))
                personaje.Acciones.Add(item.Valor);
        }

        foreach (var item in habilidades)
        {
            if (personajesPorId.TryGetValue(item.PkPersonaje, out var personaje))
                personaje.Habilidades.Add(item.Valor);
        }

        return personajes;
    }

    private sealed class RelacionDto
    {
        public int PkPersonaje { get; set; }
        public string Valor { get; set; } = string.Empty;
    }
}
