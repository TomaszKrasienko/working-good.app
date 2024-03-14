using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.owner.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class group_14_03_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                schema: "owner",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "owner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "owner",
                        principalTable: "Owner",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                schema: "owner",
                table: "Users",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                schema: "owner",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                schema: "owner",
                table: "Users",
                column: "GroupId",
                principalSchema: "owner",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                schema: "owner",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "owner");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupId",
                schema: "owner",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupId",
                schema: "owner",
                table: "Users");
        }
    }
}
