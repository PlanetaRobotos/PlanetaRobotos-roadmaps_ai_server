using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class user_purchase_email : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveEmail",
                table: "UserPurchases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3d1fd80e-4499-43b9-b732-5c34c9594885", "AQAAAAIAAYagAAAAEBrJRu3jP6i0vE3bgz8447gGXVfC1FUB1R8LwvfYNJDeo7hJ4vnQSoPUmmmGgHabrg==", "c1ff2baf-8cc2-4dec-9c7b-6df98c5cdf72" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveEmail",
                table: "UserPurchases");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f520ee26-f4e5-4293-be9d-9760cfcd8f3b", "AQAAAAIAAYagAAAAEFsq02nDW9ts6jRzfvdMWQaHv3j277z9SUxAIFHQ6egPcZUg9hyvZbQ/89wfYBwpGA==", "e653880c-26ab-4ed6-b255-bc135a9cd4de" });
        }
    }
}
