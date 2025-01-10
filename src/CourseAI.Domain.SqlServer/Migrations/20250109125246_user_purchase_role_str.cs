using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class user_purchase_role_str : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "UserPurchases",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "66a900bc-eb32-4702-971b-882ec0977f74", "AQAAAAIAAYagAAAAEBUDLgB56b7twkHZhoBTXDCFVXEQpkJaF5WRjCyIcM3LupMZpEeRPB5BZoOhO5ZqeA==", "30da5247-3c6e-4c49-b24a-5dfa1b909787" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "UserPurchases",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3d1fd80e-4499-43b9-b732-5c34c9594885", "AQAAAAIAAYagAAAAEBrJRu3jP6i0vE3bgz8447gGXVfC1FUB1R8LwvfYNJDeo7hJ4vnQSoPUmmmGgHabrg==", "c1ff2baf-8cc2-4dec-9c7b-6df98c5cdf72" });
        }
    }
}
