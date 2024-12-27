using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class authorid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Roadmaps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05b18cd1-3eec-44c9-b7e2-cd26ca7625e8", "AQAAAAIAAYagAAAAEGU6efQVsL34O7CD7WZzSB1CQRpY+I24hsNh/8wc0cRQaDqubsQ1a5TN8tkvaY3iVw==", "567790e0-bc9e-44ce-b21a-270eae406982" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Roadmaps");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "566695a6-db64-4d96-9e28-8ce6c83fe710", "AQAAAAIAAYagAAAAEJOSitE5IjrsHTx6jQpzZAMXf8Fj4Kufvfvi2Gl18jB2dSh6CUhuza9K235mb/6iIQ==", "ccef5f21-ed53-463e-9287-d122932d006e" });
        }
    }
}
