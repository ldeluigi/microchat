using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.DataAccess.Migrations;

public partial class AddNameAndSurnameIndexes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Users_Name",
            schema: "domain",
            table: "Users",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Surname",
            schema: "domain",
            table: "Users",
            column: "Surname");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Users_Name",
            schema: "domain",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_Surname",
            schema: "domain",
            table: "Users");
    }
}
