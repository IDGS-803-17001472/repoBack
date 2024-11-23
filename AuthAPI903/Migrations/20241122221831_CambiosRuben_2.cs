using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class CambiosRuben_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OportunidadVentas",
                table: "OportunidadVentas");

            migrationBuilder.RenameTable(
                name: "OportunidadVentas",
                newName: "OportunidadesVenta");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OportunidadesVenta",
                table: "OportunidadesVenta",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OportunidadesVenta",
                table: "OportunidadesVenta");

            migrationBuilder.RenameTable(
                name: "OportunidadesVenta",
                newName: "OportunidadVentas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OportunidadVentas",
                table: "OportunidadVentas",
                column: "Id");
        }
    }
}
