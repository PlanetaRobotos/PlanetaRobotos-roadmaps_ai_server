using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c7f35428-51a1-45f9-8f32-4975449c5a4f", "AQAAAAIAAYagAAAAEIgjFYm+GopOro6ZtPYU0t21tNTFVcaTXfgOcEgZOLpwsr86cBsH2Qei+fZ6oVlyjw==", "7f8a43ae-d63e-4076-b2b7-4503271d9b14" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "066fec0b-fe2c-4be2-b899-de399e16cfdc", "AQAAAAIAAYagAAAAEI+XuFgZ5bzqqyUGqGCT4xZj2LLJWNrdYykY5+SVXHWrhoM1SD9omBy2UpZxXy/3XA==", "adda4d44-c022-46ee-a4b9-11a0e5712db1" });
        }
    }
}
