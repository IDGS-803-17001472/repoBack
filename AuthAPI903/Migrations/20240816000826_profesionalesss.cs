#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // El nombre de tipo solo contiene caracteres ASCII en minúsculas. Estos nombres pueden reservarse para el idioma.
    public partial class profesionalesss : Migration
#pragma warning restore CS8981 // El nombre de tipo solo contiene caracteres ASCII en minúsculas. Estos nombres pueden reservarse para el idioma.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfesionalId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProfesionalId",
                table: "Usuarios",
                column: "ProfesionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Profesionales_ProfesionalId",
                table: "Usuarios",
                column: "ProfesionalId",
                principalTable: "Profesionales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Profesionales_ProfesionalId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ProfesionalId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ProfesionalId",
                table: "Usuarios");
        }
    }
}
