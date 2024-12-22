using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class userlikes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "566695a6-db64-4d96-9e28-8ce6c83fe710", "AQAAAAIAAYagAAAAEJOSitE5IjrsHTx6jQpzZAMXf8Fj4Kufvfvi2Gl18jB2dSh6CUhuza9K235mb/6iIQ==", "ccef5f21-ed53-463e-9287-d122932d006e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8bf3755c-6322-47c0-82e6-ac75bf848ded", "AQAAAAIAAYagAAAAEJGNtRTJ2yEtaA1KPwFTQTHfA6iDUBO4IF+mb+fPEqyvOVviR3oUdj2eR+FYw+9aiA==", "8e95b08c-c915-4a1e-bef9-33ef165bf84e" });
        }
    }
}
