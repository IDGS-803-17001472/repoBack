using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class profesionalesss : Migration
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
