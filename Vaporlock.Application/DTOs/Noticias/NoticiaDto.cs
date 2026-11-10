using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Vaporlock.Application.DTOs.Noticias;

public class NoticiaDto
{
    public int PkNoticia { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Resumen { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaPublicacion { get; set; }
    public bool Publicado { get; set; }
}

public class CrearNoticiaDto
{
    public int? FkMediaPortada { get; set; }

    [Required]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;

    public string? Resumen { get; set; }

    [Required]
    public string Contenido { get; set; } = string.Empty;

    public DateTime? FechaPublicacion { get; set; }

    public bool Publicado { get; set; }
}

public class ActualizarNoticiaDto
{
    public int? FkMediaPortada { get; set; }

    [Required]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;

    public string? Resumen { get; set; }

    [Required]
    public string Contenido { get; set; } = string.Empty;

    public DateTime? FechaPublicacion { get; set; }

    public bool Publicado { get; set; }
}

public class ActualizarNoticiaParcialDto
{
    public JsonElement FkMediaPortada { get; set; }
    public JsonElement Titulo { get; set; }
    public JsonElement Slug { get; set; }
    public JsonElement Resumen { get; set; }
    public JsonElement Contenido { get; set; }
    public JsonElement FechaPublicacion { get; set; }
    public JsonElement Publicado { get; set; }
}