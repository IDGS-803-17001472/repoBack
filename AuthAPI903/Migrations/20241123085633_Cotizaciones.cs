using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class Cotizaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cliente",
                table: "Cotizaciones");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Cotizaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_ClienteId",
                table: "Cotizaciones",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cotizaciones_Clientes_ClienteId",
                table: "Cotizaciones",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cotizaciones_Clientes_ClienteId",
                table: "Cotizaciones");

            migrationBuilder.DropIndex(
                name: "IX_Cotizaciones_ClienteId",
                table: "Cotizaciones");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Cotizaciones");

            migrationBuilder.AddColumn<string>(
                name: "Cliente",
                table: "Cotizaciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
