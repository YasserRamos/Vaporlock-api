namespace Vaporlock.Application.DTOs.Media;

public class CategoriaMediaDto
{
    public int PkCategoriaMedia { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}