using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelsDb.Migrations
{
    public partial class AddTableCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "account",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_CurrencyId",
                table: "account",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_account_currency_CurrencyId",
                table: "account",
                column: "CurrencyId",
                principalTable: "currency",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_currency_CurrencyId",
                table: "account");

            migrationBuilder.DropTable(
                name: "currency");

            migrationBuilder.DropIndex(
                name: "IX_account_CurrencyId",
                table: "account");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "account");
        }
    }
}
