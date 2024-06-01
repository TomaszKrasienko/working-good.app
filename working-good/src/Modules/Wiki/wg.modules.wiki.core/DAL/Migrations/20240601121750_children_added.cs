using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.wiki.core.DAL.Migrations
{
    /// <inheritdoc />
    public partial class children_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SectionId",
                schema: "wiki",
                table: "Sections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionId",
                schema: "wiki",
                table: "Sections",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Sections_SectionId",
                schema: "wiki",
                table: "Sections",
                column: "SectionId",
                principalSchema: "wiki",
                principalTable: "Sections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Sections_SectionId",
                schema: "wiki",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_SectionId",
                schema: "wiki",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "wiki",
                table: "Sections");
        }
    }
}
