using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.tickets.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedStatusFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateChangeDate",
                schema: "tickets",
                table: "Tickets",
                newName: "StatusChangeDate");

            migrationBuilder.RenameColumn(
                name: "State",
                schema: "tickets",
                table: "Tickets",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusChangeDate",
                schema: "tickets",
                table: "Tickets",
                newName: "StateChangeDate");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "tickets",
                table: "Tickets",
                newName: "State");
        }
    }
}
