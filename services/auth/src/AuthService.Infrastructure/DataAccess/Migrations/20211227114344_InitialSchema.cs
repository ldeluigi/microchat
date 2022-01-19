using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthService.Infrastructure.DataAccess.Migrations;

public partial class InitialSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "entities");

        migrationBuilder.CreateTable(
            name: "Accounts",
            schema: "entities",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Creation = table.Column<DateTime>(type: "datetime", nullable: false),
                Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                EmailUpdate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                PasswordRecoveryToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                PasswordRecoveryTokenExpiration = table.Column<DateTime>(type: "datetime", nullable: true),
                ConfirmationToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                ConfirmationTokenExpiration = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Sessions",
            schema: "entities",
            columns: table => new
            {
                RefreshToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                AccessTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sessions", x => x.RefreshToken);
                table.ForeignKey(
                    name: "FK_Sessions_Accounts_AccountId",
                    column: x => x.AccountId,
                    principalSchema: "entities",
                    principalTable: "Accounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Sessions_AccountId",
            schema: "entities",
            table: "Sessions",
            column: "AccountId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Sessions",
            schema: "entities");

        migrationBuilder.DropTable(
            name: "Accounts",
            schema: "entities");
    }
}
