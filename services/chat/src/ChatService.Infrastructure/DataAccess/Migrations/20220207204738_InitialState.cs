using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatService.Infrastructure.DataAccess.Migrations;

public partial class InitialState : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "domain");

        migrationBuilder.CreateTable(
            name: "PrivateChats",
            schema: "domain",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PartecipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PrivateChats", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PrivateMessages",
            schema: "domain",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SendTime = table.Column<DateTime>(type: "datetime", nullable: true),
                LastEditTime = table.Column<DateTime>(type: "datetime", nullable: true),
                Viewed = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PrivateMessages", x => x.Id)
                    .Annotation("SqlServer:Clustered", false);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "domain",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

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

        migrationBuilder.CreateIndex(
            name: "IX_PrivateMessages_ChatId",
            schema: "domain",
            table: "PrivateMessages",
            column: "ChatId");

        migrationBuilder.CreateIndex(
            name: "IX_PrivateMessages_SendTime",
            schema: "domain",
            table: "PrivateMessages",
            column: "SendTime")
            .Annotation("SqlServer:Clustered", true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PrivateChats",
            schema: "domain");

        migrationBuilder.DropTable(
            name: "PrivateMessages",
            schema: "domain");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "domain");
    }
}
