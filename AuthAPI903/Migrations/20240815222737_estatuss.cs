using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class estatuss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estatus",
                table: "Profesionales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Estatus",
                table: "Pacientes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "Profesionales");

            migrationBuilder.DropColumn(
                name: "Estatus",
                table: "Pacientes");
        }
    }
}
