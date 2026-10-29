namespace Vaporlock.Domain.Entities;

public class Historia
{
    public int PkHistoria { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public int Orden { get; set; } = 1;
    public bool Publicado { get; set; }
    public Media? MediaPortada { get; set; }
}