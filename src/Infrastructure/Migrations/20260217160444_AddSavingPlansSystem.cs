using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JuliGastos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSavingPlansSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "financial_commitments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    frequency = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    next_due_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_commitments", x => x.id);
                    table.ForeignKey(
                        name: "FK_financial_commitments_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_financial_commitments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recurring_incomes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    frequency = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: true),
                    next_expected_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recurring_incomes", x => x.id);
                    table.ForeignKey(
                        name: "FK_recurring_incomes_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_recurring_incomes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saving_plans",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    target_amount = table.Column<decimal>(type: "numeric(19,4)", precision: 19, scale: 4, nullable: false),
                    target_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    icon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "🎯"),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false, defaultValue: "#6366F1"),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Active"),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saving_plans", x => x.id);
                    table.ForeignKey(
                        name: "FK_saving_plans_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "saving_plan_accounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    saving_plan_id = table.Column<long>(type: "bigint", nullable: false),
                    account_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saving_plan_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_saving_plan_accounts_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_saving_plan_accounts_saving_plans_saving_plan_id",
                        column: x => x.saving_plan_id,
                        principalTable: "saving_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_financial_commitments_category_id",
                table: "financial_commitments",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_financial_commitments_is_active",
                table: "financial_commitments",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_financial_commitments_user_id",
                table: "financial_commitments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_financial_commitments_uuid",
                table: "financial_commitments",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recurring_incomes_category_id",
                table: "recurring_incomes",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_incomes_is_active",
                table: "recurring_incomes",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_incomes_user_id",
                table: "recurring_incomes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_incomes_uuid",
                table: "recurring_incomes",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_saving_plan_accounts_account_id",
                table: "saving_plan_accounts",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_saving_plan_accounts_saving_plan_id",
                table: "saving_plan_accounts",
                column: "saving_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_saving_plan_accounts_saving_plan_id_account_id",
                table: "saving_plan_accounts",
                columns: new[] { "saving_plan_id", "account_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_saving_plans_status",
                table: "saving_plans",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_saving_plans_user_id",
                table: "saving_plans",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_saving_plans_uuid",
                table: "saving_plans",
                column: "uuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "financial_commitments");

            migrationBuilder.DropTable(
                name: "recurring_incomes");

            migrationBuilder.DropTable(
                name: "saving_plan_accounts");

            migrationBuilder.DropTable(
                name: "saving_plans");
        }
    }
}
