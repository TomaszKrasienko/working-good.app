using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.wiki.core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "wiki");

            migrationBuilder.CreateTable(
                name: "Sections",
                schema: "wiki",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Sections_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "wiki",
                        principalTable: "Sections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Name",
                schema: "wiki",
                table: "Sections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ParentId",
                schema: "wiki",
                table: "Sections",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sections",
                schema: "wiki");
        }
    }
}
