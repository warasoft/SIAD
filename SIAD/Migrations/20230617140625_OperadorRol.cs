using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIAD.Migrations
{
    public partial class OperadorRol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id ='d033ac61-df68-45f5-ac84-d42dffdc12cc')
            BEGIN
                INSERT AspNetRoles (Id, [Name], [NormalizedName])
                VALUES ('d033ac61-df68-45f5-ac84-d42dffdc12cc','operador','OPERADOR')
            END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles Where Id='d033ac61-df68-45f5-ac84-d42dffdc12cc'");
        }
    }
}
