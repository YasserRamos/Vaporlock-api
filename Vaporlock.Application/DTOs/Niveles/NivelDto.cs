using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Vaporlock.Application.DTOs.Niveles;

public class NivelDto
{
    public int PkNivel { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Orden { get; set; }
    public bool Publicado { get; set; }
}

public class CrearNivelDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;

    public int? FkMediaPortada { get; set; }

    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int Orden { get; set; } = 1;

    public bool Publicado { get; set; }
}

public class ActualizarNivelDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;

    public int? FkMediaPortada { get; set; }

    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int Orden { get; set; }

    public bool Publicado { get; set; }
}

public class ActualizarNivelParcialDto
{
    public JsonElement Nombre { get; set; }
    public JsonElement Slug { get; set; }
    public JsonElement FkMediaPortada { get; set; }
    public JsonElement Descripcion { get; set; }
    public JsonElement Orden { get; set; }
    public JsonElement Publicado { get; set; }
}