using System.Text.Json;
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
            sql, new { Id = id });
    }

    public async Task<int> CrearAsync(CrearCategoriaMediaDto dto)
    {
        const string sql = """
            INSERT INTO dbo.CategoriasMedia (Nombre, Descripcion)
            OUTPUT INSERTED.PkCategoriaMedia
            VALUES (@Nombre, @Descripcion);
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.QuerySingleAsync<int>(sql, dto);
    }

    public async Task<bool> ActualizarAsync(
        int id, ActualizarCategoriaMediaDto dto)
    {
        const string sql = """
            UPDATE dbo.CategoriasMedia
            SET Nombre = @Nombre,
                Descripcion = @Descripcion
            WHERE PkCategoriaMedia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteAsync(
            sql,
            new { Id = id, dto.Nombre, dto.Descripcion }) > 0;
    }

    public async Task<bool> ActualizarParcialAsync(
        int id, ActualizarCategoriaMediaParcialDto dto)
    {
        var campos = new List<string>();
        var parametros = new DynamicParameters();

        parametros.Add("Id", id);

        if (dto.Nombre.ValueKind != JsonValueKind.Undefined)
        {
            campos.Add("Nombre = @Nombre");
            parametros.Add("Nombre", dto.Nombre.GetString()?.Trim());
        }

        if (dto.Descripcion.ValueKind != JsonValueKind.Undefined)
        {
            campos.Add("Descripcion = @Descripcion");
            parametros.Add(
                "Descripcion",
                dto.Descripcion.ValueKind == JsonValueKind.Null
                    ? null
                    : dto.Descripcion.GetString());
        }

        if (campos.Count == 0)
            return false;

        string sql = $"""
            UPDATE dbo.CategoriasMedia
            SET {string.Join(", ", campos)}
            WHERE PkCategoriaMedia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteAsync(sql, parametros) > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        const string sql = """
            DELETE FROM dbo.CategoriasMedia
            WHERE PkCategoriaMedia = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}