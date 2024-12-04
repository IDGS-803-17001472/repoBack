using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class ayuda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mediciones_Entradas_EntradaId",
                table: "Mediciones");

            migrationBuilder.DropIndex(
                name: "IX_Mediciones_EntradaId",
                table: "Mediciones");

            migrationBuilder.DropColumn(
                name: "EntradaId",
                table: "Mediciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntradaId",
                table: "Mediciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mediciones_EntradaId",
                table: "Mediciones",
                column: "EntradaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mediciones_Entradas_EntradaId",
                table: "Mediciones",
                column: "EntradaId",
                principalTable: "Entradas",
                principalColumn: "Id");
        }
    }
}
