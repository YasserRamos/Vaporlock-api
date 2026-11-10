using System.Text.Json;
using Vaporlock.Application.DTOs.Personajes;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

namespace Vaporlock.Application.Services;

public class PersonajesService : IPersonajesService
{
    private readonly IPersonajesRepository _repository;

    public PersonajesService(IPersonajesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonajeDto>> ObtenerPublicadosAsync()
    {
        return _repository.ObtenerPublicadosAsync();
    }

    public Task<PersonajeDto?> ObtenerPorSlugAsync(string slug)
    {
        return _repository.ObtenerPorSlugAsync(slug);
    }

    public Task<IEnumerable<PersonajeDto>> ObtenerTodosAsync()
    {
        return _repository.ObtenerTodosAsync();
    }

    public Task<PersonajeDto?> ObtenerPorIdAsync(int id)
    {
        return _repository.ObtenerPorIdAsync(id);
    }

    public Task<int> CrearAsync(CrearPersonajeDto dto)
    {
        ValidarDatosPrincipales(
            dto.Nombre,
            dto.Slug,
            dto.Rol,
            dto.Descripcion,
            dto.Agresividad
        );

        ValidarColecciones(
            dto.Apariciones,
            dto.Acciones,
            dto.Habilidades
        );

        return _repository.CrearAsync(dto);
    }

    public Task<bool> ActualizarAsync(
        int id,
        ActualizarPersonajeDto dto)
    {
        ValidarDatosPrincipales(
            dto.Nombre,
            dto.Slug,
            dto.Rol,
            dto.Descripcion,
            dto.Agresividad
        );

        ValidarColecciones(
            dto.Apariciones,
            dto.Acciones,
            dto.Habilidades
        );

        return _repository.ActualizarAsync(id, dto);
    }

    public Task<bool> ActualizarParcialAsync(
        int id,
        ActualizarPersonajeParcialDto dto)
    {
        ValidarParcial(dto);

        return _repository.ActualizarParcialAsync(id, dto);
    }

    public Task<bool> EliminarAsync(int id)
    {
        return _repository.EliminarAsync(id);
    }

    // VALIDACIONES GENERALES

    private static void ValidarDatosPrincipales(
        string nombre,
        string slug,
        string rol,
        string? descripcion,
        string? agresividad)
    {
        ValidarTextoObligatorio(nombre, "Nombre", 150);
        ValidarTextoObligatorio(slug, "Slug", 170);
        ValidarTextoObligatorio(rol, "Rol", 50);

        ValidarTextoOpcional(descripcion, "Descripcion", 1000);
        ValidarTextoOpcional(agresividad, "Agresividad", 50);
    }

    private static void ValidarTextoObligatorio(
        string? valor,
        string campo,
        int longitudMaxima)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException(
                $"{campo} es obligatorio."
            );
        }

        if (valor.Length > longitudMaxima)
        {
            throw new ArgumentException(
                $"{campo} no puede superar los {longitudMaxima} caracteres."
            );
        }
    }

    private static void ValidarTextoOpcional(
        string? valor,
        string campo,
        int longitudMaxima)
    {
        if (valor is not null && valor.Length > longitudMaxima)
        {
            throw new ArgumentException(
                $"{campo} no puede superar los {longitudMaxima} caracteres."
            );
        }
    }

    private static void ValidarColecciones(
        IEnumerable<string> apariciones,
        IEnumerable<string> acciones,
        IEnumerable<string> habilidades)
    {
        ValidarLista(apariciones, "Apariciones", 150);
        ValidarLista(acciones, "Acciones", 200);
        ValidarLista(habilidades, "Habilidades", 200);
    }

    private static void ValidarLista(
        IEnumerable<string>? valores,
        string campo,
        int longitudMaxima)
    {
        if (valores is null)
            throw new ArgumentException(
                $"{campo} no puede ser null."
            );

        foreach (var valor in valores)
        {
            if (valor is null)
            {
                throw new ArgumentException(
                    $"Los elementos de {campo} no pueden ser null."
                );
            }

            if (valor.Length > longitudMaxima)
            {
                throw new ArgumentException(
                    $"Cada elemento de {campo} puede tener máximo {longitudMaxima} caracteres."
                );
            }
        }
    }

    // VALIDACIONES PATCH

    private static void ValidarParcial(
        ActualizarPersonajeParcialDto dto)
    {
        if (dto.Nombre.ValueKind != JsonValueKind.Undefined)
        {
            ValidarTextoJsonObligatorio(
                dto.Nombre,
                "Nombre",
                150
            );
        }

        if (dto.Slug.ValueKind != JsonValueKind.Undefined)
        {
            ValidarTextoJsonObligatorio(
                dto.Slug,
                "Slug",
                170
            );
        }

        if (dto.Rol.ValueKind != JsonValueKind.Undefined)
        {
            ValidarTextoJsonObligatorio(
                dto.Rol,
                "Rol",
                50
            );
        }

        if (dto.Descripcion.ValueKind != JsonValueKind.Undefined)
        {
            ValidarTextoJsonOpcional(
                dto.Descripcion,
                "Descripcion",
                1000
            );
        }

        if (dto.Historia.ValueKind != JsonValueKind.Undefined)
        {
            ValidarTextoJsonOpcional(
                dto.Historia,
                "Historia",
                null
            );
        }

        if (dto.Agresividad.ValueKind != JsonValueKind.Undefined)
        {
            ValidarTextoJsonOpcional(
                dto.Agresividad,
                "Agresividad",
                50
            );
        }

        if (dto.FkMediaPortada.ValueKind != JsonValueKind.Undefined)
        {
            ValidarEnteroJsonNullable(
                dto.FkMediaPortada,
                "FkMediaPortada"
            );
        }

        if (dto.PuntosSalud.ValueKind != JsonValueKind.Undefined)
        {
            ValidarEnteroJsonNullable(
                dto.PuntosSalud,
                "PuntosSalud"
            );
        }

        if (dto.Publicado.ValueKind != JsonValueKind.Undefined &&
            dto.Publicado.ValueKind is not JsonValueKind.True
                and not JsonValueKind.False)
        {
            throw new ArgumentException(
                "Publicado debe ser un booleano."
            );
        }

        if (dto.Apariciones.ValueKind != JsonValueKind.Undefined)
        {
            ValidarListaJson(
                dto.Apariciones,
                "Apariciones",
                150
            );
        }

        if (dto.Acciones.ValueKind != JsonValueKind.Undefined)
        {
            ValidarListaJson(
                dto.Acciones,
                "Acciones",
                200
            );
        }

        if (dto.Habilidades.ValueKind != JsonValueKind.Undefined)
        {
            ValidarListaJson(
                dto.Habilidades,
                "Habilidades",
                200
            );
        }

        if (TodosLosCamposOmitidos(dto))
        {
            throw new ArgumentException(
                "Debes proporcionar al menos un campo para actualizar."
            );
        }
    }

    private static void ValidarTextoJsonObligatorio(
        JsonElement elemento,
        string campo,
        int longitudMaxima)
    {
        if (elemento.ValueKind != JsonValueKind.String)
        {
            throw new ArgumentException(
                $"{campo} debe ser un texto."
            );
        }

        ValidarTextoObligatorio(
            elemento.GetString(),
            campo,
            longitudMaxima
        );
    }

    private static void ValidarTextoJsonOpcional(
        JsonElement elemento,
        string campo,
        int? longitudMaxima)
    {
        if (elemento.ValueKind == JsonValueKind.Null)
            return;

        if (elemento.ValueKind != JsonValueKind.String)
        {
            throw new ArgumentException(
                $"{campo} debe ser un texto o null."
            );
        }

        var valor = elemento.GetString();

        if (longitudMaxima.HasValue)
        {
            ValidarTextoOpcional(
                valor,
                campo,
                longitudMaxima.Value
            );
        }
    }

    private static void ValidarEnteroJsonNullable(
        JsonElement elemento,
        string campo)
    {
        if (elemento.ValueKind == JsonValueKind.Null)
            return;

        if (elemento.ValueKind != JsonValueKind.Number ||
            !elemento.TryGetInt32(out _))
        {
            throw new ArgumentException(
                $"{campo} debe ser un entero o null."
            );
        }
    }

    private static void ValidarListaJson(
        JsonElement elemento,
        string campo,
        int longitudMaxima)
    {
        if (elemento.ValueKind == JsonValueKind.Null)
            return;

        if (elemento.ValueKind != JsonValueKind.Array)
        {
            throw new ArgumentException(
                $"{campo} debe ser un arreglo o null."
            );
        }

        foreach (var item in elemento.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.String)
            {
                throw new ArgumentException(
                    $"Todos los elementos de {campo} deben ser textos."
                );
            }

            var valor = item.GetString();

            if (valor is not null && valor.Length > longitudMaxima)
            {
                throw new ArgumentException(
                    $"Cada elemento de {campo} puede tener máximo {longitudMaxima} caracteres."
                );
            }
        }
    }

    private static bool TodosLosCamposOmitidos(
        ActualizarPersonajeParcialDto dto)
    {
        return
            dto.FkMediaPortada.ValueKind == JsonValueKind.Undefined &&
            dto.Nombre.ValueKind == JsonValueKind.Undefined &&
            dto.Slug.ValueKind == JsonValueKind.Undefined &&
            dto.Descripcion.ValueKind == JsonValueKind.Undefined &&
            dto.Historia.ValueKind == JsonValueKind.Undefined &&
            dto.Rol.ValueKind == JsonValueKind.Undefined &&
            dto.PuntosSalud.ValueKind == JsonValueKind.Undefined &&
            dto.Agresividad.ValueKind == JsonValueKind.Undefined &&
            dto.Apariciones.ValueKind == JsonValueKind.Undefined &&
            dto.Acciones.ValueKind == JsonValueKind.Undefined &&
            dto.Habilidades.ValueKind == JsonValueKind.Undefined &&
            dto.Publicado.ValueKind == JsonValueKind.Undefined;
    }
}