using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAD.Migrations
{
    public partial class ConsultorRol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id ='5ffc6393-6cb3-4fb8-a337-7cb4d0446e56')
            BEGIN
                INSERT AspNetRoles (Id, [Name], [NormalizedName])
                VALUES ('5ffc6393-6cb3-4fb8-a337-7cb4d0446e56','consultor','CONSULTOR')
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles Where Id='5ffc6393-6cb3-4fb8-a337-7cb4d0446e56'");
        }
    }
}
