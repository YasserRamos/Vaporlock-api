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
        const string sql = """
            SELECT
                PkPersonaje,
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Historia
            FROM dbo.Personajes
            WHERE Publicado = 1
            ORDER BY PkPersonaje;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<PersonajeDto>(sql);
    }

    public async Task<PersonajeDto?> ObtenerPorSlugAsync(string slug)
    {
        const string sql = """
            SELECT
                PkPersonaje,
                FkMediaPortada,
                Nombre,
                Slug,
                Descripcion,
                Historia
            FROM dbo.Personajes
            WHERE Slug = @Slug
              AND Publicado = 1;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<PersonajeDto>(
            sql,
            new { Slug = slug }
        );
    }
}