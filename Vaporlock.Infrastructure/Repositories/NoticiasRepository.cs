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
                FechaPublicacion
            FROM dbo.Noticias
            WHERE Publicado = 1
              AND (FechaPublicacion IS NULL
                   OR FechaPublicacion <= SYSUTCDATETIME())
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
                FechaPublicacion
            FROM dbo.Noticias
            WHERE Slug = @Slug
              AND Publicado = 1
              AND (FechaPublicacion IS NULL
                   OR FechaPublicacion <= SYSUTCDATETIME());
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<NoticiaDto>(
            sql,
            new { Slug = slug }
        );
    }
}