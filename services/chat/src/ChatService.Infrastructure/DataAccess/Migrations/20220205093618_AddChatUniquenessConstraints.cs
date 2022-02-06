using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatService.Infrastructure.DataAccess.Migrations;

public partial class AddChatUniquenessConstraints : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_PrivateChats_CreatorId",
            schema: "domain",
            table: "PrivateChats");

        migrationBuilder.DropIndex(
            name: "IX_PrivateChats_PartecipantId",
            schema: "domain",
            table: "PrivateChats");

        migrationBuilder.CreateIndex(
            name: "IX_PrivateChats_CreatorId_PartecipantId",
            schema: "domain",
            table: "PrivateChats",
            columns: new[] { "CreatorId", "PartecipantId" },
            unique: true,
            filter: "[CreatorId] IS NOT NULL AND [PartecipantId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_PrivateChats_PartecipantId_CreatorId",
            schema: "domain",
            table: "PrivateChats",
            columns: new[] { "PartecipantId", "CreatorId" },
            unique: true,
            filter: "[PartecipantId] IS NOT NULL AND [CreatorId] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_PrivateChats_CreatorId_PartecipantId",
            schema: "domain",
            table: "PrivateChats");

        migrationBuilder.DropIndex(
            name: "IX_PrivateChats_PartecipantId_CreatorId",
            schema: "domain",
            table: "PrivateChats");

        migrationBuilder.CreateIndex(
            name: "IX_PrivateChats_CreatorId",
            schema: "domain",
            table: "PrivateChats",
            column: "CreatorId");

        migrationBuilder.CreateIndex(
            name: "IX_PrivateChats_PartecipantId",
            schema: "domain",
            table: "PrivateChats",
            column: "PartecipantId");
    }
}
