
using Microsoft.EntityFrameworkCore;
using Vaporlock.Domain.Entities;

namespace Vaporlock.Infrastructure.Data;

public class VaporlockDbContext : DbContext
{
    public VaporlockDbContext(
        DbContextOptions<VaporlockDbContext> options)
        : base(options)
    {
    }

    public DbSet<CategoriaMedia> CategoriasMedia => Set<CategoriaMedia>();
    public DbSet<Media> Media => Set<Media>();
    public DbSet<Noticia> Noticias => Set<Noticia>();
    public DbSet<Personaje> Personajes => Set<Personaje>();
    public DbSet<Nivel> Niveles => Set<Nivel>();
    public DbSet<Historia> Historias => Set<Historia>();
    public DbSet<MiembroEquipo> MiembrosEquipo => Set<MiembroEquipo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CategoriasMedia
        modelBuilder.Entity<CategoriaMedia>(entity =>
        {
            entity.ToTable("CategoriasMedia");

            entity.HasKey(x => x.PkCategoriaMedia);

            entity.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Descripcion)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Nombre)
                .IsUnique();
        });

        // Media
        modelBuilder.Entity<Media>(entity =>
        {
            entity.ToTable("Media");

            entity.HasKey(x => x.PkMedia);

            entity.Property(x => x.NombreArchivo)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.ContentType)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Datos)
                .HasColumnType("varbinary(max)")
                .IsRequired();

            entity.Property(x => x.TextoAlternativo)
                .HasMaxLength(300);

            entity.Property(x => x.FechaCreacion)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.HasOne(x => x.Categoria)
                .WithMany(x => x.Medias)
                .HasForeignKey(x => x.FkCategoriaMedia)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.FkCategoriaMedia);
        });

        // Noticias
        modelBuilder.Entity<Noticia>(entity =>
        {
            entity.ToTable("Noticias");

            entity.HasKey(x => x.PkNoticia);

            entity.Property(x => x.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(220)
                .IsRequired();

            entity.Property(x => x.Resumen)
                .HasMaxLength(500);

            entity.Property(x => x.Contenido)
                .IsRequired();

            entity.Property(x => x.FechaCreacion)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(x => x.Publicado)
                .HasDefaultValue(false);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasIndex(x => new
            {
                x.Publicado,
                x.FechaPublicacion
            });

            entity.HasOne(x => x.MediaPortada)
                .WithMany()
                .HasForeignKey(x => x.FkMediaPortada)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Personajes
        modelBuilder.Entity<Personaje>(entity =>
        {
            entity.ToTable("Personajes");

            entity.HasKey(x => x.PkPersonaje);

            entity.Property(x => x.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(170)
                .IsRequired();

            entity.Property(x => x.Descripcion)
                .HasMaxLength(1000);

            entity.Property(x => x.Publicado)
                .HasDefaultValue(false);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasIndex(x => x.Publicado);

            entity.HasOne(x => x.MediaPortada)
                .WithMany()
                .HasForeignKey(x => x.FkMediaPortada)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Niveles
        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.ToTable("Niveles", table =>
            {
                table.HasCheckConstraint(
                    "CK_Niveles_Orden",
                    "[Orden] >= 1");
            });

            entity.HasKey(x => x.PkNivel);

            entity.Property(x => x.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(170)
                .IsRequired();

            entity.Property(x => x.Orden)
                .HasDefaultValue(1);

            entity.Property(x => x.Publicado)
                .HasDefaultValue(false);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasIndex(x => new { x.Publicado, x.Orden });

            entity.HasOne(x => x.MediaPortada)
                .WithMany()
                .HasForeignKey(x => x.FkMediaPortada)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Historia
        modelBuilder.Entity<Historia>(entity =>
        {
            entity.ToTable("Historias", table =>
            {
                table.HasCheckConstraint(
                    "CK_Historias_Orden",
                    "[Orden] >= 1");
            });

            entity.HasKey(x => x.PkHistoria);

            entity.Property(x => x.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(220)
                .IsRequired();

            entity.Property(x => x.Contenido)
                .IsRequired();

            entity.Property(x => x.Orden)
                .HasDefaultValue(1);

            entity.Property(x => x.Publicado)
                .HasDefaultValue(false);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasIndex(x => new { x.Publicado, x.Orden });

            entity.HasOne(x => x.MediaPortada)
                .WithMany()
                .HasForeignKey(x => x.FkMediaPortada)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // MiembrosEquipo
        modelBuilder.Entity<MiembroEquipo>(entity =>
        {
            entity.ToTable("MiembrosEquipo", table =>
            {
                table.HasCheckConstraint(
                    "CK_MiembrosEquipo_Orden",
                    "[Orden] >= 1");
            });

            entity.HasKey(x => x.PkMiembroEquipo);

            entity.Property(x => x.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Rol)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Biografia)
                .HasMaxLength(1000);

            entity.Property(x => x.Enlace)
                .HasMaxLength(500);

            entity.Property(x => x.Orden)
                .HasDefaultValue(1);

            entity.Property(x => x.Publicado)
                .HasDefaultValue(true);

            entity.HasIndex(x => new { x.Publicado, x.Orden });

            entity.HasOne(x => x.MediaPortada)
                .WithMany()
                .HasForeignKey(x => x.FkMediaPortada)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
