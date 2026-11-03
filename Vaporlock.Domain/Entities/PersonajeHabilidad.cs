namespace Vaporlock.Domain.Entities;

public class PersonajeHabilidad
{
    public int PkPersonajeHabilidad { get; set; }

    public int FkPersonaje { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public Personaje Personaje { get; set; } = null!;
}