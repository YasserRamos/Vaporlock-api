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
                M.PkMedia,
                M.FkCategoriaMedia,
                C.Nombre AS Categoria,
                M.NombreArchivo,
                M.ContentType,
                M.TextoAlternativo,
                M.FechaCreacion
            FROM dbo.Media M
            INNER JOIN dbo.CategoriasMedia C
                ON C.PkCategoriaMedia = M.FkCategoriaMedia
            ORDER BY M.PkMedia DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<MediaDto>(sql);
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

    public async Task<int> CrearAsync(
        string nombreArchivo,
        string contentType,
        byte[] datos,
        string? textoAlternativo)
    {
        const string sqlCategoria = """
            SELECT PkCategoriaMedia
            FROM dbo.CategoriasMedia
            WHERE Nombre = @Nombre;
            """;

        const string sqlInsertar = """
            INSERT INTO dbo.Media
            (
                FkCategoriaMedia,
                NombreArchivo,
                ContentType,
                Datos,
                TextoAlternativo,
                FechaCreacion
            )
            OUTPUT INSERTED.PkMedia
            VALUES
            (
                @FkCategoriaMedia,
                @NombreArchivo,
                @ContentType,
                @Datos,
                @TextoAlternativo,
                SYSUTCDATETIME()
            );
            """;

        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var categoriaId = await connection.QuerySingleOrDefaultAsync<int?>(
                sqlCategoria,
                new { Nombre = "Noticias" },
                transaction
            );

            if (categoriaId == null)
            {
                throw new InvalidOperationException(
                    "No existe la categoría de Media llamada 'Noticias'."
                );
            }

            var id = await connection.QuerySingleAsync<int>(
                sqlInsertar,
                new
                {
                    FkCategoriaMedia = categoriaId.Value,
                    NombreArchivo = nombreArchivo,
                    ContentType = contentType,
                    Datos = datos,
                    TextoAlternativo = textoAlternativo
                },
                transaction
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
}