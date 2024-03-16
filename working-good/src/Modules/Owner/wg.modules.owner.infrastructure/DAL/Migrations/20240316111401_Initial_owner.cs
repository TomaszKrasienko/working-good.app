using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.owner.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial_owner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "owner");

            migrationBuilder.CreateTable(
                name: "Owner",
                schema: "owner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "owner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    FullName_FirstName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    FullName_LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationToken_Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VerificationToken_VerificationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ResetPasswordToken_Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResetPasswordToken_Expiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Owner_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "owner",
                        principalTable: "Owner",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupMembership",
                schema: "owner",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembership", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupMembership_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalSchema: "owner",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembership_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "owner",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembership_UsersId",
                schema: "owner",
                table: "GroupMembership",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                schema: "owner",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "owner",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OwnerId",
                schema: "owner",
                table: "Users",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMembership",
                schema: "owner");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "owner");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "owner");

            migrationBuilder.DropTable(
                name: "Owner",
                schema: "owner");
        }
    }
}
