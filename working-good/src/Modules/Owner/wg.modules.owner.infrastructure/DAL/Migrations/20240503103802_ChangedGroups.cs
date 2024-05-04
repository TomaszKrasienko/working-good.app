using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.owner.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembership_Groups_GroupsId",
                schema: "owner",
                table: "GroupMembership");

            migrationBuilder.RenameColumn(
                name: "GroupsId",
                schema: "owner",
                table: "GroupMembership",
                newName: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembership_Groups_GroupId",
                schema: "owner",
                table: "GroupMembership",
                column: "GroupId",
                principalSchema: "owner",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembership_Groups_GroupId",
                schema: "owner",
                table: "GroupMembership");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "owner",
                table: "GroupMembership",
                newName: "GroupsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembership_Groups_GroupsId",
                schema: "owner",
                table: "GroupMembership",
                column: "GroupsId",
                principalSchema: "owner",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
