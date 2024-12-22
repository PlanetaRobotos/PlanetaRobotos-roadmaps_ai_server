using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseAI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class userlikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "UserRoadmaps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserLikes",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikes", x => new { x.UserId, x.RoadmapId });
                    table.ForeignKey(
                        name: "FK_UserLikes_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Roadmaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8bf3755c-6322-47c0-82e6-ac75bf848ded", "AQAAAAIAAYagAAAAEJGNtRTJ2yEtaA1KPwFTQTHfA6iDUBO4IF+mb+fPEqyvOVviR3oUdj2eR+FYw+9aiA==", "8e95b08c-c915-4a1e-bef9-33ef165bf84e" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikes_RoadmapId",
                table: "UserLikes",
                column: "RoadmapId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikes");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "UserRoadmaps");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ed29b2a-f688-43cd-91ff-d25e10bb1023", "AQAAAAIAAYagAAAAEOyKMM/OX0ENYbyTS6iJ23YAIX4iRcXj8/sS6hkMMKyJo5u1nPETExsaOFOz1VueHw==", "76cf46f8-0bcb-4b5b-9dbc-5dfc48a6adff" });
        }
    }
}
