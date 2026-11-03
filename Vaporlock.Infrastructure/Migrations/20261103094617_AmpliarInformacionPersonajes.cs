using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vaporlock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AmpliarInformacionPersonajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Agresividad",
                table: "Personajes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PuntosSalud",
                table: "Personajes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Personajes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Sin clasificar");

            migrationBuilder.CreateTable(
                name: "PersonajesAcciones",
                columns: table => new
                {
                    PkPersonajeAccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkPersonaje = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonajesAcciones", x => x.PkPersonajeAccion);
                    table.ForeignKey(
                        name: "FK_PersonajesAcciones_Personajes_FkPersonaje",
                        column: x => x.FkPersonaje,
                        principalTable: "Personajes",
                        principalColumn: "PkPersonaje",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonajesApariciones",
                columns: table => new
                {
                    PkPersonajeAparicion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkPersonaje = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonajesApariciones", x => x.PkPersonajeAparicion);
                    table.ForeignKey(
                        name: "FK_PersonajesApariciones_Personajes_FkPersonaje",
                        column: x => x.FkPersonaje,
                        principalTable: "Personajes",
                        principalColumn: "PkPersonaje",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonajesHabilidades",
                columns: table => new
                {
                    PkPersonajeHabilidad = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkPersonaje = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonajesHabilidades", x => x.PkPersonajeHabilidad);
                    table.ForeignKey(
                        name: "FK_PersonajesHabilidades_Personajes_FkPersonaje",
                        column: x => x.FkPersonaje,
                        principalTable: "Personajes",
                        principalColumn: "PkPersonaje",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonajesAcciones_FkPersonaje",
                table: "PersonajesAcciones",
                column: "FkPersonaje");

            migrationBuilder.CreateIndex(
                name: "IX_PersonajesApariciones_FkPersonaje",
                table: "PersonajesApariciones",
                column: "FkPersonaje");

            migrationBuilder.CreateIndex(
                name: "IX_PersonajesHabilidades_FkPersonaje",
                table: "PersonajesHabilidades",
                column: "FkPersonaje");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonajesAcciones");

            migrationBuilder.DropTable(
                name: "PersonajesApariciones");

            migrationBuilder.DropTable(
                name: "PersonajesHabilidades");

            migrationBuilder.DropColumn(
                name: "Agresividad",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "PuntosSalud",
                table: "Personajes");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Personajes");
        }
    }
}
