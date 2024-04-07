using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.companies.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class is_active : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "companies",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "companies",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "companies",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "companies",
                table: "Companies");
        }
    }
}
