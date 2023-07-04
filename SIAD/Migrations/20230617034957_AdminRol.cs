using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAD.Migrations
{
    public partial class AdminRol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id ='c07f7c69-e3bd-43b2-91aa-db6b2f4838b4')
            BEGIN
                INSERT AspNetRoles (Id, [Name], [NormalizedName])
                VALUES ('c07f7c69-e3bd-43b2-91aa-db6b2f4838b4','admin','ADMIN')
            END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles Where Id='c07f7c69-e3bd-43b2-91aa-db6b2f4838b4'");
        }
    }
}
