namespace Vaporlock.Domain.Entities;

public class Nivel
{
    public int PkNivel { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Orden { get; set; } = 1;
    public bool Publicado { get; set; }
    public Media? MediaPortada { get; set; }
}