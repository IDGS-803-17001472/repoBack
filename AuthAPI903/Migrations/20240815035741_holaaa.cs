#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // El nombre de tipo solo contiene caracteres ASCII en minúsculas. Estos nombres pueden reservarse para el idioma.
    public partial class holaaa : Migration
#pragma warning restore CS8981 // El nombre de tipo solo contiene caracteres ASCII en minúsculas. Estos nombres pueden reservarse para el idioma.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Usuarios_IdUsuario",
                table: "Pacientes");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Pacientes",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Pacientes_IdUsuario",
                table: "Pacientes",
                newName: "IX_Pacientes_UsuarioId");

            migrationBuilder.AddColumn<int>(
                name: "IdPersona",
                table: "Pacientes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_IdPersona",
                table: "Pacientes",
                column: "IdPersona",
                unique: true,
                filter: "[IdPersona] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Personas_IdPersona",
                table: "Pacientes",
                column: "IdPersona",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Usuarios_UsuarioId",
                table: "Pacientes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Personas_IdPersona",
                table: "Pacientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Usuarios_UsuarioId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_IdPersona",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "IdPersona",
                table: "Pacientes");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Pacientes",
                newName: "IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_Pacientes_UsuarioId",
                table: "Pacientes",
                newName: "IX_Pacientes_IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Usuarios_IdUsuario",
                table: "Pacientes",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
