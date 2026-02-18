using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuliGastos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSavingPlansSystemModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LinkedAt",
                table: "saving_plan_accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "recurring_incomes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDate",
                table: "recurring_incomes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                table: "recurring_incomes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "financial_commitments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDate",
                table: "financial_commitments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                table: "financial_commitments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedAt",
                table: "saving_plan_accounts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "recurring_incomes");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "recurring_incomes");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "recurring_incomes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "financial_commitments");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "financial_commitments");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "financial_commitments");
        }
    }
}
