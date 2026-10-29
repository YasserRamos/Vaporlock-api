namespace Vaporlock.Domain.Entities;

public class Noticia
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
    public Media? MediaPortada { get; set; }
}