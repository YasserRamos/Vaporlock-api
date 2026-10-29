
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Vaporlock.Infrastructure.Data;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly SqlConnectionFactory _connectionFactory;

    public HealthController(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            message = "La API de Vaporlock está funcionando."
        });
    }

    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();

            await connection.ExecuteScalarAsync<int>("SELECT 1");

            return Ok(new
            {
                status = "ok",
                database = "connected",
                message = "Conexión con SQL Server establecida."
            });
        }
        catch (SqlException)
        {
            return StatusCode(503, new
            {
                status = "error",
                message = "No se pudo conectar con la base de datos."
            });
        }
        catch (InvalidOperationException)
        {
            return StatusCode(503, new
            {
                status = "error",
                message = "La conexión a la base de datos no está configurada correctamente."
            });
        }
    }
}
