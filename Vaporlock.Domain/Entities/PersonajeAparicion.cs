namespace Vaporlock.Domain.Entities;

public class PersonajeAparicion
{
    public int PkPersonajeAparicion { get; set; }

    public int FkPersonaje { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public Personaje Personaje { get; set; } = null!;
}