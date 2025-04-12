using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class user_firstname_to_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppSumoLicenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LicenseKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tier = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSumoLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSumoLicenses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "Name", "PasswordHash", "SecurityStamp" },
                values: new object[] { "622535ce-7b6d-41a7-8c38-c02d37e58d9b", null, "AQAAAAIAAYagAAAAEHWWuKDaMkAAAJTBO8uxZccHQclLso4P80+/uJg6p6kcksh8P5jEcT2oudFJ9MsoQw==", "6993c769-4cda-470c-89fc-d95d20c2195c" });

            migrationBuilder.CreateIndex(
                name: "IX_AppSumoLicenses_UserId",
                table: "AppSumoLicenses",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSumoLicenses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "FirstName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "44ecd088-53c5-44d8-bf1f-479c723ad6a5", null, "AQAAAAIAAYagAAAAEBKA7hyVxo7HTolA44pEW/Y7eL+2pI3VERYTQcWWLWxQ3km83/tYb5YLBL0D+NZOxw==", "74b7ba34-4083-498d-9e41-bb828fd733a0" });
        }
    }
}
