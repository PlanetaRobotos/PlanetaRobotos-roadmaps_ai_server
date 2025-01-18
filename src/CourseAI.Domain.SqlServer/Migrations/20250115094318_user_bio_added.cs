using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class user_bio_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Bio", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { null, "44ecd088-53c5-44d8-bf1f-479c723ad6a5", "AQAAAAIAAYagAAAAEBKA7hyVxo7HTolA44pEW/Y7eL+2pI3VERYTQcWWLWxQ3km83/tYb5YLBL0D+NZOxw==", "74b7ba34-4083-498d-9e41-bb828fd733a0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f4dd407-1e20-4240-b940-e431b9f1a03b", "AQAAAAIAAYagAAAAEF6xXBHsz/Ub0ztSIKJ8mSKB6bdYEbYsZPx4OsbPFsxpP3FPkf2Ujqemzriwz60YdA==", "d1c504e1-92d1-45f0-8e0a-5a5a82bcd647" });
        }
    }
}
