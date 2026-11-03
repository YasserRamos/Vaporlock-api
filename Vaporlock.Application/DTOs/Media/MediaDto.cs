namespace Vaporlock.Application.DTOs.Media;

public class MediaDto
{
    public int PkMedia { get; set; }
    public int FkCategoriaMedia { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string NombreArchivo { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string? TextoAlternativo { get; set; }
    public DateTime FechaCreacion { get; set; }
}