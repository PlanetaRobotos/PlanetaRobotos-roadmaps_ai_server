using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Roadmaps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Roadmaps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ColorHex = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryCourses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCourses_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Roadmaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryRelations_Categories_ChildCategoryId",
                        column: x => x.ChildCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryRelations_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseTypeRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTypeRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTypeRelations_CourseTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CourseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTypeRelations_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Roadmaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "41200e6c-680d-41b6-b709-83ef9f500a0d", "AQAAAAIAAYagAAAAEDjmrOjWmZVVpYuv0cQrRAIEPXQRUTxCKJvm5nZlnCYcb9eY2E9DD6anTWwL/u303g==", "88913392-a01c-4f99-aaa9-543cc4b50bac" });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Order",
                table: "Categories",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCourses_CategoryId_Order",
                table: "CategoryCourses",
                columns: new[] { "CategoryId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCourses_CategoryId_RoadmapId",
                table: "CategoryCourses",
                columns: new[] { "CategoryId", "RoadmapId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCourses_RoadmapId",
                table: "CategoryCourses",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRelations_ChildCategoryId",
                table: "CategoryRelations",
                column: "ChildCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRelations_ParentCategoryId_ChildCategoryId",
                table: "CategoryRelations",
                columns: new[] { "ParentCategoryId", "ChildCategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRelations_ParentCategoryId_Order",
                table: "CategoryRelations",
                columns: new[] { "ParentCategoryId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTypeRelations_RoadmapId_TypeId",
                table: "CourseTypeRelations",
                columns: new[] { "RoadmapId", "TypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTypeRelations_TypeId",
                table: "CourseTypeRelations",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTypes_Name",
                table: "CourseTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTypes_Order",
                table: "CourseTypes",
                column: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCourses");

            migrationBuilder.DropTable(
                name: "CategoryRelations");

            migrationBuilder.DropTable(
                name: "CourseTypeRelations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CourseTypes");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Roadmaps");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "172fc443-d027-4650-8df5-e56f58162ea5", "AQAAAAIAAYagAAAAEDcma0hgrmdMB1tL0L0lT1VV1IJ8G3tJnrIlGPTCJw6XnJrLKsxppAfcKyndB3rk1w==", "c25ed91c-44e2-41a6-9a20-df41037f7a28" });
        }
    }
}
