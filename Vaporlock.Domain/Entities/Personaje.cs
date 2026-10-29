namespace Vaporlock.Domain.Entities;

public class Personaje
{
    public int PkPersonaje { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Historia { get; set; }
    public bool Publicado { get; set; }
    public Media? MediaPortada { get; set; }
}