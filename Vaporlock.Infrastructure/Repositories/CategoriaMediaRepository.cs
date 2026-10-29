using Dapper;
using Vaporlock.Application.DTOs.Media;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Infrastructure.Repositories;

public class CategoriaMediaRepository : ICategoriaMediaRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public CategoriaMediaRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<CategoriaMediaDto>> ObtenerTodosAsync()
    {
        const string sql = """
            SELECT PkCategoriaMedia, Nombre, Descripcion
            FROM dbo.CategoriasMedia
            ORDER BY Nombre;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryAsync<CategoriaMediaDto>(sql);
    }

    public async Task<CategoriaMediaDto?> ObtenerPorIdAsync(int id)
    {
        const string sql = """
            SELECT PkCategoriaMedia, Nombre, Descripcion
            FROM dbo.CategoriasMedia
            WHERE PkCategoriaMedia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<CategoriaMediaDto>(
            sql,
            new { Id = id }
        );
    }
}