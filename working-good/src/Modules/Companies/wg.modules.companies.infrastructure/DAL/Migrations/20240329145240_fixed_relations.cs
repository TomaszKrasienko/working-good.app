using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wg.modules.companies.infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixed_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId1",
                schema: "companies",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Companies_CompanyId1",
                schema: "companies",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CompanyId1",
                schema: "companies",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyId1",
                schema: "companies",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                schema: "companies",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                schema: "companies",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId1",
                schema: "companies",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId1",
                schema: "companies",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompanyId1",
                schema: "companies",
                table: "Projects",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId1",
                schema: "companies",
                table: "Employees",
                column: "CompanyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyId1",
                schema: "companies",
                table: "Employees",
                column: "CompanyId1",
                principalSchema: "companies",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Companies_CompanyId1",
                schema: "companies",
                table: "Projects",
                column: "CompanyId1",
                principalSchema: "companies",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
