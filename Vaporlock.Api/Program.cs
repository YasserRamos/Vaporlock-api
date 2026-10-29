
using Microsoft.EntityFrameworkCore;
using Vaporlock.Infrastructure.Data;
using Vaporlock.Application.Services;
using Vaporlock.Infrastructure.Repositories;
using Vaporlock.Application.Interfaces.Repositories;
using Vaporlock.Application.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Controladores y documentación de la API
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// CORS para el frontend React con Vite
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Conexión para Dapper
builder.Services.AddSingleton<SqlConnectionFactory>();

// Conexión para Entity Framework Core
builder.Services.AddDbContext<VaporlockDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Vaporlock")
        ?? throw new InvalidOperationException(
            "No se encontró la cadena de conexión 'Vaporlock'."
        )
    )
);

// Módulo Media
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();

// Módulo Categorías de Media
builder.Services.AddScoped<ICategoriaMediaRepository, CategoriaMediaRepository>();
builder.Services.AddScoped<ICategoriaMediaService, CategoriaMediaService>();

// Módulo Noticias
builder.Services.AddScoped<INoticiasRepository, NoticiasRepository>();
builder.Services.AddScoped<INoticiasService, NoticiasService>();

// Módulo Personajes
builder.Services.AddScoped<IPersonajesRepository, PersonajesRepository>();
builder.Services.AddScoped<IPersonajesService, PersonajesService>();

// Módulo Niveles
builder.Services.AddScoped<INivelesRepository, NivelesRepository>();
builder.Services.AddScoped<INivelesService, NivelesService>();

// Módulo Historia
builder.Services.AddScoped<IHistoriasRepository, HistoriasRepository>();
builder.Services.AddScoped<IHistoriasService, HistoriasService>();

// Módulo Miembros del equipo
builder.Services.AddScoped<IMiembrosEquipoRepository, MiembrosEquipoRepository>();
builder.Services.AddScoped<IMiembrosEquipoService, MiembrosEquipoService>();

var app = builder.Build();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Permitir solicitudes desde React
app.UseCors("ReactFrontend");

app.MapControllers();

app.Run();
