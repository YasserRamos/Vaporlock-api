namespace Vaporlock.Domain.Entities;

public class MiembroEquipo
{
    public int PkMiembroEquipo { get; set; }
    public int? FkMediaPortada { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string? Biografia { get; set; }
    public string? Enlace { get; set; }
    public int Orden { get; set; } = 1;
    public bool Publicado { get; set; } = true;
    public Media? MediaPortada { get; set; }
}