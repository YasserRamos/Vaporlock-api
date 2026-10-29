namespace Vaporlock.Domain.Entities;

public class CategoriaMedia
{
    public int PkCategoriaMedia { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public ICollection<Media> Medias { get; set; } = new List<Media>();
}