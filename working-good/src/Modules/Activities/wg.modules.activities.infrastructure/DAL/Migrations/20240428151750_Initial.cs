using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.activities.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "activities");

            migrationBuilder.CreateTable(
                name: "DailyUserActivities",
                schema: "activities",
                columns: table => new
                {
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyUserActivities", x => new { x.Day, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityTimeFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivityTimeTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DailyUserActivityDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DailyUserActivityUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    type = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_DailyUserActivities_DailyUserActivityDay_DailyUserActivityUserId",
                        columns: x => new { x.DailyUserActivityDay, x.DailyUserActivityUserId },
                        principalSchema: "activities",
                        principalTable: "DailyUserActivities",
                        principalColumns: new[] { "Day", "UserId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DailyUserActivityDay_DailyUserActivityUserId",
                schema: "activities",
                table: "Activities",
                columns: new[] { "DailyUserActivityDay", "DailyUserActivityUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities",
                schema: "activities");

            migrationBuilder.DropTable(
                name: "DailyUserActivities",
                schema: "activities");
        }
    }
}
