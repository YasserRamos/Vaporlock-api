using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Vaporlock.Application.DTOs.Historias;

public class HistoriaDto
{
    public int PkHistoria { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public int Orden { get; set; }
    public bool Publicado { get; set; }
}

public class CrearHistoriaDto
{
    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(180)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    public string Contenido { get; set; } = string.Empty;

    public int? FkMediaPortada { get; set; }

    [Range(1, int.MaxValue)]
    public int Orden { get; set; } = 1;

    public bool Publicado { get; set; }
}

public class ActualizarHistoriaDto
{
    [Required]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(180)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    public string Contenido { get; set; } = string.Empty;

    public int? FkMediaPortada { get; set; }

    [Range(1, int.MaxValue)]
    public int Orden { get; set; }

    public bool Publicado { get; set; }
}

public class ActualizarHistoriaParcialDto
{
    public JsonElement Titulo { get; set; }
    public JsonElement Slug { get; set; }
    public JsonElement Contenido { get; set; }
    public JsonElement FkMediaPortada { get; set; }
    public JsonElement Orden { get; set; }
    public JsonElement Publicado { get; set; }
}