using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuliGastos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUuidToAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "uuid",
                table: "accounts",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_uuid",
                table: "accounts",
                column: "uuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_accounts_uuid",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "uuid",
                table: "accounts");
        }
    }
}
