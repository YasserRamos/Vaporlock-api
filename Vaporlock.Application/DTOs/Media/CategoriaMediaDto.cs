using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Vaporlock.Application.DTOs.Media;

public class CategoriaMediaDto
{
    public int PkCategoriaMedia { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class CrearCategoriaMediaDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }
}

public class ActualizarCategoriaMediaDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }
}

public class ActualizarCategoriaMediaParcialDto
{
    public JsonElement Nombre { get; set; }
    public JsonElement Descripcion { get; set; }
}