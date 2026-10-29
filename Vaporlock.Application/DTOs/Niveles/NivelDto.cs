namespace Vaporlock.Application.DTOs.Niveles;

public class NivelDto
{
    public int PkNivel { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int Orden { get; set; }
}