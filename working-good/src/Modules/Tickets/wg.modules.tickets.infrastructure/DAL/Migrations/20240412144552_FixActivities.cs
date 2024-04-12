using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.tickets.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Tickets_TicketId",
                schema: "tickets",
                table: "Activity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                schema: "tickets",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "Activity",
                schema: "tickets",
                newName: "Activities",
                newSchema: "tickets");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_TicketId",
                schema: "tickets",
                table: "Activities",
                newName: "IX_Activities_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                schema: "tickets",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Tickets_TicketId",
                schema: "tickets",
                table: "Activities",
                column: "TicketId",
                principalSchema: "tickets",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Tickets_TicketId",
                schema: "tickets",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                schema: "tickets",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Activities",
                schema: "tickets",
                newName: "Activity",
                newSchema: "tickets");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_TicketId",
                schema: "tickets",
                table: "Activity",
                newName: "IX_Activity_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                schema: "tickets",
                table: "Activity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Tickets_TicketId",
                schema: "tickets",
                table: "Activity",
                column: "TicketId",
                principalSchema: "tickets",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
