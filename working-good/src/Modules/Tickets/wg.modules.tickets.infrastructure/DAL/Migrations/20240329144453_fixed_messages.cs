using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.tickets.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixed_messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Tickets_TicketId1",
                schema: "tickets",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_TicketId1",
                schema: "tickets",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TicketId1",
                schema: "tickets",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TicketId1",
                schema: "tickets",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_TicketId1",
                schema: "tickets",
                table: "Messages",
                column: "TicketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Tickets_TicketId1",
                schema: "tickets",
                table: "Messages",
                column: "TicketId1",
                principalSchema: "tickets",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
