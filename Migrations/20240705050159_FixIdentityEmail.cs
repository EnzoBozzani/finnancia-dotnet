using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinnanciaCSharp.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c3c6bf5-3b51-4447-ba9a-6334c2cdaa7f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d290cdd9-8f27-42a3-bd95-316a36349c7f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d05573a-79f6-45ae-9e21-fdcad5bcfbb1", null, "Admin", "ADMIN" },
                    { "8758378a-0a89-4c30-8339-b8b0031a9e4a", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d05573a-79f6-45ae-9e21-fdcad5bcfbb1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8758378a-0a89-4c30-8339-b8b0031a9e4a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7c3c6bf5-3b51-4447-ba9a-6334c2cdaa7f", null, "User", "USER" },
                    { "d290cdd9-8f27-42a3-bd95-316a36349c7f", null, "Admin", "ADMIN" }
                });
        }
    }
}
