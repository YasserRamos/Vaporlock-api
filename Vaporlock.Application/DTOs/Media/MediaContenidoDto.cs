namespace Vaporlock.Application.DTOs.Media;

public class MediaContenidoDto
{
    public int PkMedia { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Datos { get; set; } = [];
    public string? TextoAlternativo { get; set; }
}