using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class categorierel_add_thumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d5b3fafe-e1e1-40c2-9ad5-2c80dc029f59", "AQAAAAIAAYagAAAAEIubEHqMpVXcYeYNY0NPmnj/JH8KZddpx02wVw4awVkPIVbCwce7DgCiF6GaEp9l2w==", "0962c00f-0cea-41ce-bcd5-3e08c03e1730" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0ecc9fbf-031d-43ed-ba6d-2667f044749f", "AQAAAAIAAYagAAAAEF9SPUHcOMljpBmkUh3feu0PPKK441BRkdlmpH8WXQvZWujMUKAF6+QAlelKyDU0Kg==", "07c51bec-85eb-4926-9647-1ea62f5bbc3f" });
        }
    }
}
