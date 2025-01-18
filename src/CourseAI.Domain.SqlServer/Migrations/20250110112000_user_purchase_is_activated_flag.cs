using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class user_purchase_is_activated_flag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "UserPurchases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "172fc443-d027-4650-8df5-e56f58162ea5", "AQAAAAIAAYagAAAAEDcma0hgrmdMB1tL0L0lT1VV1IJ8G3tJnrIlGPTCJw6XnJrLKsxppAfcKyndB3rk1w==", "c25ed91c-44e2-41a6-9a20-df41037f7a28" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "UserPurchases");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "66a900bc-eb32-4702-971b-882ec0977f74", "AQAAAAIAAYagAAAAEBUDLgB56b7twkHZhoBTXDCFVXEQpkJaF5WRjCyIcM3LupMZpEeRPB5BZoOhO5ZqeA==", "30da5247-3c6e-4c49-b24a-5dfa1b909787" });
        }
    }
}
