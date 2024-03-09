using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.owner.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Users_Owner_OwnerId1",
                        column: x => x.OwnerId1,
                        principalSchema: "owner",
                        principalTable: "Owner",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_OwnerId1",
                schema: "owner",
                table: "Users",
                column: "OwnerId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "owner");

            migrationBuilder.DropTable(
                name: "Owner",
                schema: "owner");
        }
    }
}
