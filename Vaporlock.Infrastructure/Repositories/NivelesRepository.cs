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
                Orden
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
                Orden
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
}