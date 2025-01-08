using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class user_purchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderReference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPurchases", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f520ee26-f4e5-4293-be9d-9760cfcd8f3b", "AQAAAAIAAYagAAAAEFsq02nDW9ts6jRzfvdMWQaHv3j277z9SUxAIFHQ6egPcZUg9hyvZbQ/89wfYBwpGA==", "e653880c-26ab-4ed6-b255-bc135a9cd4de" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPurchases");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e3c3d9e9-deea-46e5-b9d2-c836a6456696", "AQAAAAIAAYagAAAAEG00Ry3MkUcDhGpJ+qV0+rbJhKdI3ApBItgkvEm8rgU1NcyB6N334fTOgcJYB/lcZw==", "30fbd42c-f004-4f5a-a9d0-fc49d99d99e0" });
        }
    }
}
