using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class authoridnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AuthorId",
                table: "Roadmaps",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1d4befea-077b-40f5-a5e2-8c1952af9041", "AQAAAAIAAYagAAAAED3cWfIc1HQYYfjnXZNdd20gfWK6UV2e14wdbWRJK0k8V5tbmbn0RQxjjmSN1ihnMQ==", "203b0d8e-790f-4cf5-85a1-35478bd30371" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AuthorId",
                table: "Roadmaps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "05b18cd1-3eec-44c9-b7e2-cd26ca7625e8", "AQAAAAIAAYagAAAAEGU6efQVsL34O7CD7WZzSB1CQRpY+I24hsNh/8wc0cRQaDqubsQ1a5TN8tkvaY3iVw==", "567790e0-bc9e-44ce-b21a-270eae406982" });
        }
    }
}
