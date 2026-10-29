using Dapper;
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

    public async Task<IEnumerable<HistoriaDto>> ObtenerPublicadasAsync()
    {
        const string sql = """
            SELECT
                PkHistoria,
                FkMediaPortada,
                Titulo,
                Slug,
                Contenido,
                Orden
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
                Orden
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
}