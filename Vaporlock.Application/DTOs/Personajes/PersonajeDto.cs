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
}