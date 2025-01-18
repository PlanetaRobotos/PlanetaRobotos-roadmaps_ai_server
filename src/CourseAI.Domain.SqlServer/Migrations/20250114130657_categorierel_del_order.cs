using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class categorierel_del_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoryRelations_ParentCategoryId_Order",
                table: "CategoryRelations");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "CategoryRelations");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0ecc9fbf-031d-43ed-ba6d-2667f044749f", "AQAAAAIAAYagAAAAEF9SPUHcOMljpBmkUh3feu0PPKK441BRkdlmpH8WXQvZWujMUKAF6+QAlelKyDU0Kg==", "07c51bec-85eb-4926-9647-1ea62f5bbc3f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CategoryRelations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c1632975-e7e9-4a70-912a-c8b434e8cbee", "AQAAAAIAAYagAAAAEBERV3yFyq6PxqXo3h+X2gMKRTAYGZ17h+ZaSByLY8DOg8eb42m7LEIoG2gsGXlFKA==", "6e17b0ce-bc9b-4a85-9857-eddaee4089e1" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRelations_ParentCategoryId_Order",
                table: "CategoryRelations",
                columns: new[] { "ParentCategoryId", "Order" });
        }
    }
}
