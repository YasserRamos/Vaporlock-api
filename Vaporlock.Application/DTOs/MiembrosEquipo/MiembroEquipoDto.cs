using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Vaporlock.Application.DTOs.MiembrosEquipo;

public class MiembroEquipoDto
{
    public int PkMiembroEquipo { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public string? Enlace { get; set; }
    public int Orden { get; set; }
    public bool Publicado { get; set; }
}

public class CrearMiembroEquipoDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Rol { get; set; } = string.Empty;

    public int? FkMediaPortada { get; set; }

    public string? Biografia { get; set; }

    public string? Enlace { get; set; }

    [Range(1, int.MaxValue)]
    public int Orden { get; set; } = 1;

    public bool Publicado { get; set; }
}

public class ActualizarMiembroEquipoDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Rol { get; set; } = string.Empty;

    public int? FkMediaPortada { get; set; }

    public string? Biografia { get; set; }

    public string? Enlace { get; set; }

    [Range(1, int.MaxValue)]
    public int Orden { get; set; }

    public bool Publicado { get; set; }
}

public class ActualizarMiembroEquipoParcialDto
{
    public JsonElement Nombre { get; set; }
    public JsonElement Rol { get; set; }
    public JsonElement FkMediaPortada { get; set; }
    public JsonElement Biografia { get; set; }
    public JsonElement Enlace { get; set; }
    public JsonElement Orden { get; set; }
    public JsonElement Publicado { get; set; }
}