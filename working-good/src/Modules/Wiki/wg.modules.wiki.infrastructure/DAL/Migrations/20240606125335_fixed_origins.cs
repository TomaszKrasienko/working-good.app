using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.wiki.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixed_origins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Sections_SectionId",
                schema: "wiki",
                table: "Note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                schema: "wiki",
                table: "Note");

            migrationBuilder.RenameTable(
                name: "Note",
                schema: "wiki",
                newName: "Notes",
                newSchema: "wiki");

            migrationBuilder.RenameIndex(
                name: "IX_Note_SectionId",
                schema: "wiki",
                table: "Notes",
                newName: "IX_Notes_SectionId");

            migrationBuilder.AddColumn<string>(
                name: "OriginId",
                schema: "wiki",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginType",
                schema: "wiki",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                schema: "wiki",
                table: "Notes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Sections_SectionId",
                schema: "wiki",
                table: "Notes",
                column: "SectionId",
                principalSchema: "wiki",
                principalTable: "Sections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Sections_SectionId",
                schema: "wiki",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                schema: "wiki",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "OriginId",
                schema: "wiki",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "OriginType",
                schema: "wiki",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "Notes",
                schema: "wiki",
                newName: "Note",
                newSchema: "wiki");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_SectionId",
                schema: "wiki",
                table: "Note",
                newName: "IX_Note_SectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                schema: "wiki",
                table: "Note",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Sections_SectionId",
                schema: "wiki",
                table: "Note",
                column: "SectionId",
                principalSchema: "wiki",
                principalTable: "Sections",
                principalColumn: "Id");
        }
    }
}
