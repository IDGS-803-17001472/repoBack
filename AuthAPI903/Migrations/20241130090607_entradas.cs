using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class entradas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entradas_Pacientes_IdPaciente",
                table: "Entradas");

            migrationBuilder.DropForeignKey(
                name: "FK_Mediciones_Entradas_IdEntrada",
                table: "Mediciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Entradas_Pacientes_IdPaciente",
                table: "Entradas",
                column: "IdPaciente",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mediciones_Entradas_IdEntrada",
                table: "Mediciones",
                column: "IdEntrada",
                principalTable: "Entradas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entradas_Pacientes_IdPaciente",
                table: "Entradas");

            migrationBuilder.DropForeignKey(
                name: "FK_Mediciones_Entradas_IdEntrada",
                table: "Mediciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Entradas_Pacientes_IdPaciente",
                table: "Entradas",
                column: "IdPaciente",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mediciones_Entradas_IdEntrada",
                table: "Mediciones",
                column: "IdEntrada",
                principalTable: "Entradas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
