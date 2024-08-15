using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthAPI903.Migrations
{
    /// <inheritdoc />
    public partial class migracionjeje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdAppUser",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdAppUser",
                table: "Usuarios",
                column: "IdAppUser",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_AspNetUsers_IdAppUser",
                table: "Usuarios",
                column: "IdAppUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_AspNetUsers_IdAppUser",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_IdAppUser",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdAppUser",
                table: "Usuarios");
        }
    }
}
