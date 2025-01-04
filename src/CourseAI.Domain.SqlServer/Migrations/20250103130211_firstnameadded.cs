using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class firstnameadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "FirstName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e3c3d9e9-deea-46e5-b9d2-c836a6456696", null, "AQAAAAIAAYagAAAAEG00Ry3MkUcDhGpJ+qV0+rbJhKdI3ApBItgkvEm8rgU1NcyB6N334fTOgcJYB/lcZw==", "30fbd42c-f004-4f5a-a9d0-fc49d99d99e0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c7f35428-51a1-45f9-8f32-4975449c5a4f", "AQAAAAIAAYagAAAAEIgjFYm+GopOro6ZtPYU0t21tNTFVcaTXfgOcEgZOLpwsr86cBsH2Qei+fZ6oVlyjw==", "7f8a43ae-d63e-4076-b2b7-4503271d9b14" });
        }
    }
}
