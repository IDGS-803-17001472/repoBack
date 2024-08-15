using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class diarioss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entradas_Usuarios_IdUsuario",
                table: "Entradas");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Entradas",
                newName: "IdPaciente");

            migrationBuilder.RenameIndex(
                name: "IX_Entradas_IdUsuario",
                table: "Entradas",
                newName: "IX_Entradas_IdPaciente");

            migrationBuilder.AddColumn<int>(
                name: "EntradaId",
                table: "Mediciones",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Entradas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Contenido",
                table: "Entradas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Mediciones_EntradaId",
                table: "Mediciones",
                column: "EntradaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entradas_Pacientes_IdPaciente",
                table: "Entradas",
                column: "IdPaciente",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mediciones_Entradas_EntradaId",
                table: "Mediciones",
                column: "EntradaId",
                principalTable: "Entradas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entradas_Pacientes_IdPaciente",
                table: "Entradas");

            migrationBuilder.DropForeignKey(
                name: "FK_Mediciones_Entradas_EntradaId",
                table: "Mediciones");

            migrationBuilder.DropIndex(
                name: "IX_Mediciones_EntradaId",
                table: "Mediciones");

            migrationBuilder.DropColumn(
                name: "EntradaId",
                table: "Mediciones");

            migrationBuilder.RenameColumn(
                name: "IdPaciente",
                table: "Entradas",
                newName: "IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_Entradas_IdPaciente",
                table: "Entradas",
                newName: "IX_Entradas_IdUsuario");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Entradas",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Contenido",
                table: "Entradas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entradas_Usuarios_IdUsuario",
                table: "Entradas",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
