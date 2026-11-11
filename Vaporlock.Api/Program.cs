
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Vaporlock.Infrastructure.Data;
using Vaporlock.Application.Services;
using Vaporlock.Infrastructure.Repositories;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// CONTROLADORES Y DOCUMENTACIÓN
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// CORS PARA REACT
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://vaporlock-pwa.onrender.com",
                "http://localhost:5173",
                "http://localhost:8081"

            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// CONEXIÓN DAPPER
builder.Services.AddSingleton<SqlConnectionFactory>();

// CONEXIÓN ENTITY FRAMEWORK CORE
builder.Services.AddDbContext<VaporlockDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Vaporlock")
        ?? throw new InvalidOperationException(
            "No se encontró la cadena de conexión 'Vaporlock'."
        )
    )
);

// MÓDULO MEDIA
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();

// MÓDULO CATEGORÍAS DE MEDIA
builder.Services.AddScoped<ICategoriaMediaRepository, CategoriaMediaRepository>();
builder.Services.AddScoped<ICategoriaMediaService, CategoriaMediaService>();

// MÓDULO NOTICIAS
builder.Services.AddScoped<INoticiasRepository, NoticiasRepository>();
builder.Services.AddScoped<INoticiasService, NoticiasService>();

// MÓDULO PERSONAJES
builder.Services.AddScoped<IPersonajesRepository, PersonajesRepository>();
builder.Services.AddScoped<IPersonajesService, PersonajesService>();

// MÓDULO NIVELES
builder.Services.AddScoped<INivelesRepository, NivelesRepository>();
builder.Services.AddScoped<INivelesService, NivelesService>();

// MÓDULO HISTORIAS
builder.Services.AddScoped<IHistoriasRepository, HistoriasRepository>();
builder.Services.AddScoped<IHistoriasService, HistoriasService>();

// MÓDULO MIEMBROS DEL EQUIPO
builder.Services.AddScoped<IMiembrosEquipoRepository, MiembrosEquipoRepository>();
builder.Services.AddScoped<IMiembrosEquipoService, MiembrosEquipoService>();

var app = builder.Build();

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors("ReactFrontend");

// MANEJO GLOBAL DE EXCEPCIONES
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.Clear();
        context.Response.ContentType = "application/json";

        var exceptionFeature = context.Features
            .Get<IExceptionHandlerFeature>();

        var exception = exceptionFeature?.Error;

        var logger = context.RequestServices
            .GetRequiredService<ILogger<Program>>();

        switch (exception)
        {
            case ArgumentException:
                context.Response.StatusCode =
                    StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(new
                {
                    status = 400,
                    mensaje = exception.Message
                });
                break;

            case KeyNotFoundException:
                context.Response.StatusCode =
                    StatusCodes.Status404NotFound;

                await context.Response.WriteAsJsonAsync(new
                {
                    status = 404,
                    mensaje = exception.Message
                });
                break;

            default:
                logger.LogError(
                    exception,
                    "Error no controlado durante la petición HTTP."
                );

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    status = 500,
                    mensaje = "Ocurrió un error interno en el servidor."
                });
                break;
        }
    });
});

// SWAGGER SOLO EN DESARROLLO
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CONTROLADORES
app.MapControllers();

app.Run();
