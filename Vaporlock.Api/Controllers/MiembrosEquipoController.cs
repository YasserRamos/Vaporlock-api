using Microsoft.AspNetCore.Mvc;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Api.Controllers;

[ApiController]
[Route("api/miembros-equipo")]
public class MiembrosEquipoController : ControllerBase
{
    private readonly IMiembrosEquipoService _service;

    public MiembrosEquipoController(IMiembrosEquipoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPublicados()
    {
        var miembros = await _service.ObtenerPublicadosAsync();
        return Ok(miembros);
    }
}