using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAD.Migrations
{
    public partial class addAtribUser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApellidoNombre",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeptoDiv",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApellidoNombre",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeptoDiv",
                table: "AspNetUsers");
        }
    }
}
