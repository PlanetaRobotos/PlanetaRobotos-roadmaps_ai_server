using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class categories_positioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f4dd407-1e20-4240-b940-e431b9f1a03b", "AQAAAAIAAYagAAAAEF6xXBHsz/Ub0ztSIKJ8mSKB6bdYEbYsZPx4OsbPFsxpP3FPkf2Ujqemzriwz60YdA==", "d1c504e1-92d1-45f0-8e0a-5a5a82bcd647" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d5b3fafe-e1e1-40c2-9ad5-2c80dc029f59", "AQAAAAIAAYagAAAAEIubEHqMpVXcYeYNY0NPmnj/JH8KZddpx02wVw4awVkPIVbCwce7DgCiF6GaEp9l2w==", "0962c00f-0cea-41ce-bcd5-3e08c03e1730" });
        }
    }
}
