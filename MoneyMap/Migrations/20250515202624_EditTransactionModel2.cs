using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyMap.Migrations
{
    /// <inheritdoc />
    public partial class EditTransactionModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoriesIdCategory",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoriesIdCategory",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CategoriesIdCategory",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoriesIdCategory",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoriesIdCategory",
                table: "Transactions",
                column: "CategoriesIdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoriesIdCategory",
                table: "Transactions",
                column: "CategoriesIdCategory",
                principalTable: "Categories",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
