using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuliGastos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "accounts");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "accounts",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "accounts",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "accounts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "accounts",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "MonthlyMaintenanceFee",
                table: "accounts",
                newName: "monthly_maintenance_fee");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "accounts",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CurrentBalance",
                table: "accounts",
                newName: "current_balance");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "accounts",
                newName: "currency_code");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "accounts",
                newName: "created_at");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "accounts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "monthly_maintenance_fee",
                table: "accounts",
                type: "numeric(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                defaultValue: 0.00m,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "accounts",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<decimal>(
                name: "current_balance",
                table: "accounts",
                type: "numeric(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                defaultValue: 0.0000m,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "currency_code",
                table: "accounts",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "COP",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                table: "accounts",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_user_id",
                table: "accounts",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_users_user_id",
                table: "accounts",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_users_user_id",
                table: "accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_user_id",
                table: "accounts");

            migrationBuilder.RenameTable(
                name: "accounts",
                newName: "Accounts");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Accounts",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Accounts",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Accounts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "monthly_maintenance_fee",
                table: "Accounts",
                newName: "MonthlyMaintenanceFee");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Accounts",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "current_balance",
                table: "Accounts",
                newName: "CurrentBalance");

            migrationBuilder.RenameColumn(
                name: "currency_code",
                table: "Accounts",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Accounts",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Accounts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyMaintenanceFee",
                table: "Accounts",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)",
                oldPrecision: 19,
                oldScale: 4,
                oldDefaultValue: 0.00m);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentBalance",
                table: "Accounts",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(19,4)",
                oldPrecision: 19,
                oldScale: 4,
                oldDefaultValue: 0.0000m);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "Accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldDefaultValue: "COP");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Accounts",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");
        }
    }
}
