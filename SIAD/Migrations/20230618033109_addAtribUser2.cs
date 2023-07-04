using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAD.Migrations
{
    public partial class addAtribUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "AspNetUsers",
                newName: "Grado");

            migrationBuilder.AddColumn<int>(
                name: "Matricula",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Matricula",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Grado",
                table: "AspNetUsers",
                newName: "Telefono");

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
