namespace Vaporlock.Domain.Entities;

public class Media
{
    public int PkMedia { get; set; }
    public int FkCategoriaMedia { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Datos { get; set; } = [];
    public string? TextoAlternativo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public CategoriaMedia Categoria { get; set; } = null!;
}