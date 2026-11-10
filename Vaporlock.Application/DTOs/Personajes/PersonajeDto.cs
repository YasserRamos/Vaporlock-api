using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Vaporlock.Application.DTOs.Personajes;

public class PersonajeDto
{
    public int PkPersonaje { get; set; }
    public int? FkMediaPortada { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
    public string? Historia { get; set; }

    public string Rol { get; set; } = "Sin clasificar";
    public int? PuntosSalud { get; set; }
    public string? Agresividad { get; set; }

    public string? UrlRetrato { get; set; }

    public List<string> Apariciones { get; set; } = [];
    public List<string> Acciones { get; set; } = [];
    public List<string> Habilidades { get; set; } = [];

    public bool Publicado { get; set; }
}

public class CrearPersonajeDto
{
    public int? FkMediaPortada { get; set; }

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(170)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    public string? Historia { get; set; }

    [Required]
    [StringLength(50)]
    public string Rol { get; set; } = "Sin clasificar";

    public int? PuntosSalud { get; set; }

    [StringLength(50)]
    public string? Agresividad { get; set; }

    public List<string> Apariciones { get; set; } = [];
    public List<string> Acciones { get; set; } = [];
    public List<string> Habilidades { get; set; } = [];

    public bool Publicado { get; set; }
}

public class ActualizarPersonajeDto
{
    public int? FkMediaPortada { get; set; }

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(170)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    public string? Historia { get; set; }

    [Required]
    [StringLength(50)]
    public string Rol { get; set; } = "Sin clasificar";

    public int? PuntosSalud { get; set; }

    [StringLength(50)]
    public string? Agresividad { get; set; }

    public List<string> Apariciones { get; set; } = [];
    public List<string> Acciones { get; set; } = [];
    public List<string> Habilidades { get; set; } = [];

    public bool Publicado { get; set; }
}

public class ActualizarPersonajeParcialDto
{
    public JsonElement FkMediaPortada { get; set; }
    public JsonElement Nombre { get; set; }
    public JsonElement Slug { get; set; }

    public JsonElement Descripcion { get; set; }
    public JsonElement Historia { get; set; }

    public JsonElement Rol { get; set; }
    public JsonElement PuntosSalud { get; set; }
    public JsonElement Agresividad { get; set; }

    public JsonElement Apariciones { get; set; }
    public JsonElement Acciones { get; set; }
    public JsonElement Habilidades { get; set; }

    public JsonElement Publicado { get; set; }
}