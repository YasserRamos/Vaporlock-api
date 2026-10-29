using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vaporlock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasMedia",
                columns: table => new
                {
                    PkCategoriaMedia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasMedia", x => x.PkCategoriaMedia);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    PkMedia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkCategoriaMedia = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Datos = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TextoAlternativo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.PkMedia);
                    table.ForeignKey(
                        name: "FK_Media_CategoriasMedia_FkCategoriaMedia",
                        column: x => x.FkCategoriaMedia,
                        principalTable: "CategoriasMedia",
                        principalColumn: "PkCategoriaMedia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Historias",
                columns: table => new
                {
                    PkHistoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkMediaPortada = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Publicado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historias", x => x.PkHistoria);
                    table.CheckConstraint("CK_Historias_Orden", "[Orden] >= 1");
                    table.ForeignKey(
                        name: "FK_Historias_Media_FkMediaPortada",
                        column: x => x.FkMediaPortada,
                        principalTable: "Media",
                        principalColumn: "PkMedia",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MiembrosEquipo",
                columns: table => new
                {
                    PkMiembroEquipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkMediaPortada = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Biografia = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Enlace = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Publicado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiembrosEquipo", x => x.PkMiembroEquipo);
                    table.CheckConstraint("CK_MiembrosEquipo_Orden", "[Orden] >= 1");
                    table.ForeignKey(
                        name: "FK_MiembrosEquipo_Media_FkMediaPortada",
                        column: x => x.FkMediaPortada,
                        principalTable: "Media",
                        principalColumn: "PkMedia",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Niveles",
                columns: table => new
                {
                    PkNivel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkMediaPortada = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(170)", maxLength: 170, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Publicado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Niveles", x => x.PkNivel);
                    table.CheckConstraint("CK_Niveles_Orden", "[Orden] >= 1");
                    table.ForeignKey(
                        name: "FK_Niveles_Media_FkMediaPortada",
                        column: x => x.FkMediaPortada,
                        principalTable: "Media",
                        principalColumn: "PkMedia",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Noticias",
                columns: table => new
                {
                    PkNoticia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkMediaPortada = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    Resumen = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Publicado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noticias", x => x.PkNoticia);
                    table.ForeignKey(
                        name: "FK_Noticias_Media_FkMediaPortada",
                        column: x => x.FkMediaPortada,
                        principalTable: "Media",
                        principalColumn: "PkMedia",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Personajes",
                columns: table => new
                {
                    PkPersonaje = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkMediaPortada = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(170)", maxLength: 170, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Historia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publicado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.PkPersonaje);
                    table.ForeignKey(
                        name: "FK_Personajes_Media_FkMediaPortada",
                        column: x => x.FkMediaPortada,
                        principalTable: "Media",
                        principalColumn: "PkMedia",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasMedia_Nombre",
                table: "CategoriasMedia",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Historias_FkMediaPortada",
                table: "Historias",
                column: "FkMediaPortada");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_Publicado_Orden",
                table: "Historias",
                columns: new[] { "Publicado", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_Historias_Slug",
                table: "Historias",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_FkCategoriaMedia",
                table: "Media",
                column: "FkCategoriaMedia");

            migrationBuilder.CreateIndex(
                name: "IX_MiembrosEquipo_FkMediaPortada",
                table: "MiembrosEquipo",
                column: "FkMediaPortada");

            migrationBuilder.CreateIndex(
                name: "IX_MiembrosEquipo_Publicado_Orden",
                table: "MiembrosEquipo",
                columns: new[] { "Publicado", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_Niveles_FkMediaPortada",
                table: "Niveles",
                column: "FkMediaPortada");

            migrationBuilder.CreateIndex(
                name: "IX_Niveles_Publicado_Orden",
                table: "Niveles",
                columns: new[] { "Publicado", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_Niveles_Slug",
                table: "Niveles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Noticias_FkMediaPortada",
                table: "Noticias",
                column: "FkMediaPortada");

            migrationBuilder.CreateIndex(
                name: "IX_Noticias_Publicado_FechaPublicacion",
                table: "Noticias",
                columns: new[] { "Publicado", "FechaPublicacion" });

            migrationBuilder.CreateIndex(
                name: "IX_Noticias_Slug",
                table: "Noticias",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personajes_FkMediaPortada",
                table: "Personajes",
                column: "FkMediaPortada");

            migrationBuilder.CreateIndex(
                name: "IX_Personajes_Publicado",
                table: "Personajes",
                column: "Publicado");

            migrationBuilder.CreateIndex(
                name: "IX_Personajes_Slug",
                table: "Personajes",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historias");

            migrationBuilder.DropTable(
                name: "MiembrosEquipo");

            migrationBuilder.DropTable(
                name: "Niveles");

            migrationBuilder.DropTable(
                name: "Noticias");

            migrationBuilder.DropTable(
                name: "Personajes");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "CategoriasMedia");
        }
    }
}
