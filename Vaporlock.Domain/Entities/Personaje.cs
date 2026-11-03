namespace Vaporlock.Domain.Entities;

public class Personaje
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

    public bool Publicado { get; set; }

    public Media? MediaPortada { get; set; }

    public ICollection<PersonajeAparicion> Apariciones { get; set; }
        = new List<PersonajeAparicion>();

    public ICollection<PersonajeAccion> Acciones { get; set; }
        = new List<PersonajeAccion>();

    public ICollection<PersonajeHabilidad> Habilidades { get; set; }
        = new List<PersonajeHabilidad>();
}