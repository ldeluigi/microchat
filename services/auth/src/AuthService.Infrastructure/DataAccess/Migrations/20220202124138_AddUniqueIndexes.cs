using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.DataAccess.Migrations;

public partial class AddUniqueIndexes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "domain");

        migrationBuilder.RenameTable(
            name: "Sessions",
            newName: "Sessions",
            newSchema: "domain");

        migrationBuilder.RenameTable(
            name: "Accounts",
            newName: "Accounts",
            newSchema: "domain");

        migrationBuilder.AlterColumn<string>(
            name: "Username",
            schema: "domain",
            table: "Accounts",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            schema: "domain",
            table: "Accounts",
            type: "nvarchar(450)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_Email",
            schema: "domain",
            table: "Accounts",
            column: "Email",
            unique: true,
            filter: "[Email] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_Username",
            schema: "domain",
            table: "Accounts",
            column: "Username",
            unique: true,
            filter: "[Username] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Accounts_Email",
            schema: "domain",
            table: "Accounts");

        migrationBuilder.DropIndex(
            name: "IX_Accounts_Username",
            schema: "domain",
            table: "Accounts");

        migrationBuilder.RenameTable(
            name: "Sessions",
            schema: "domain",
            newName: "Sessions");

        migrationBuilder.RenameTable(
            name: "Accounts",
            schema: "domain",
            newName: "Accounts");

        migrationBuilder.AlterColumn<string>(
            name: "Username",
            table: "Accounts",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Accounts",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)",
            oldNullable: true);
    }
}
