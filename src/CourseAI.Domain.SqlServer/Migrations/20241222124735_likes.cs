using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class likes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Roadmaps",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ed29b2a-f688-43cd-91ff-d25e10bb1023", "AQAAAAIAAYagAAAAEOyKMM/OX0ENYbyTS6iJ23YAIX4iRcXj8/sS6hkMMKyJo5u1nPETExsaOFOz1VueHw==", "76cf46f8-0bcb-4b5b-9dbc-5dfc48a6adff" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Roadmaps");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a5231cb4-2e31-41c3-ac40-5fbf97fa31d8", "AQAAAAIAAYagAAAAECLDuU/xVx3tico56wKCsfrLgmD/N5gJh/qVFH4rc/OEKuqG7HFWn2c1LZmv7gLZsQ==", "290234b7-9ff0-4d54-8e0b-49fe3a5f89dd" });
        }
    }
}
