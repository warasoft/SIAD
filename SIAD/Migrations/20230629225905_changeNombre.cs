using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAD.Migrations
{
    public partial class changeNombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApellidoNombre",
                table: "AspNetUsers",
                newName: "Nombre");

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "AspNetUsers",
                newName: "ApellidoNombre");
        }
    }
}
