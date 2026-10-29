using Dapper;
using Vaporlock.Application.DTOs.Media;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Infrastructure.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public MediaRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<MediaDto>> ObtenerTodosAsync()
    {
        const string sql = """
            SELECT
                PkMedia,
                NombreArchivo,
                ContentType,
                TextoAlternativo,
                FechaCreacion
            FROM dbo.Media
            ORDER BY PkMedia DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        var resultado = await connection.QueryAsync<MediaDto>(sql);

        return resultado;
    }

    public async Task<MediaContenidoDto?> ObtenerPorIdAsync(int id)
    {
        const string sql = """
            SELECT
                PkMedia,
                NombreArchivo,
                ContentType,
                Datos,
                TextoAlternativo
            FROM dbo.Media
            WHERE PkMedia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<MediaContenidoDto>(
            sql,
            new { Id = id }
        );
    }
}