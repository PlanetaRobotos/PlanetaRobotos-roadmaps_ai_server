using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class categories_relation_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Roadmaps");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b968a421-50f1-4fcf-a993-d16429b8bf14", "AQAAAAIAAYagAAAAEMGfU5CYVrPzLeG9Ck5FOF0oCX73BP5vpQB5+B4O0AUSIe0OdkB0SwbVjmicoRxQ/Q==", "b4fbb5b5-e0ee-4f9c-9348-91939126eae7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Roadmaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "41200e6c-680d-41b6-b709-83ef9f500a0d", "AQAAAAIAAYagAAAAEDjmrOjWmZVVpYuv0cQrRAIEPXQRUTxCKJvm5nZlnCYcb9eY2E9DD6anTWwL/u303g==", "88913392-a01c-4f99-aaa9-543cc4b50bac" });
        }
    }
}
