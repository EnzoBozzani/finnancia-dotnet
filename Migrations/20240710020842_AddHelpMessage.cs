using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinnanciaCSharp.Migrations
{
    /// <inheritdoc />
    public partial class AddHelpMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b85e51c-03ef-45eb-82dc-047003fd040e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6cea7b9-5a77-49f6-a8e5-b0ebf7740d2b");

            migrationBuilder.CreateTable(
                name: "HelpMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpMessage_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3e02ad2a-cc4b-40fa-b90a-a5d8abcf3bd0", null, "Admin", "ADMIN" },
                    { "bbf0beec-53c3-4853-b5e6-37fb6c42a12d", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelpMessage_UserId",
                table: "HelpMessage",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpMessage");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e02ad2a-cc4b-40fa-b90a-a5d8abcf3bd0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbf0beec-53c3-4853-b5e6-37fb6c42a12d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b85e51c-03ef-45eb-82dc-047003fd040e", null, "User", "USER" },
                    { "f6cea7b9-5a77-49f6-a8e5-b0ebf7740d2b", null, "Admin", "ADMIN" }
                });
        }
    }
}
