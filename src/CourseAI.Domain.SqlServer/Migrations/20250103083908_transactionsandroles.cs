using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class transactionsandroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tokens",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TokenTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenTransactions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "Tokens" },
                values: new object[] { "066fec0b-fe2c-4be2-b899-de399e16cfdc", "AQAAAAIAAYagAAAAEI+XuFgZ5bzqqyUGqGCT4xZj2LLJWNrdYykY5+SVXHWrhoM1SD9omBy2UpZxXy/3XA==", "adda4d44-c022-46ee-a4b9-11a0e5712db1", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenTransactions");

            migrationBuilder.DropColumn(
                name: "Tokens",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "06335492-098f-4fd3-ad45-a7a3530d01d0", "AQAAAAIAAYagAAAAEFBN1TrfCH0vWUhEq2OtfqKWzkli0yuz0S++Bsp+i3zuYD37w6iThCJiP7ZcA7uQxw==", "2be0dda2-8f48-4532-bd2e-63f5e2c592d3" });
        }
    }
}
