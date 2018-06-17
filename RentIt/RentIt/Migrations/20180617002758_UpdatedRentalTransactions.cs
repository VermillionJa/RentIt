using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RentIt.Migrations
{
    public partial class UpdatedRentalTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalTransactionItems_RentalTransactions_TransactionId",
                table: "RentalTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalTransactions_Customers_CustomerId",
                table: "RentalTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentalTransactions",
                table: "RentalTransactions");

            migrationBuilder.RenameTable(
                name: "RentalTransactions",
                newName: "RentalTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_RentalTransactions_CustomerId",
                table: "RentalTransaction",
                newName: "IX_RentalTransaction_CustomerId");

            migrationBuilder.AddColumn<bool>(
                name: "LateFeesPaid",
                table: "RentalTransaction",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentalTransaction",
                table: "RentalTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalTransaction_Customers_CustomerId",
                table: "RentalTransaction",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalTransactionItems_RentalTransaction_TransactionId",
                table: "RentalTransactionItems",
                column: "TransactionId",
                principalTable: "RentalTransaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalTransaction_Customers_CustomerId",
                table: "RentalTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalTransactionItems_RentalTransaction_TransactionId",
                table: "RentalTransactionItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentalTransaction",
                table: "RentalTransaction");

            migrationBuilder.DropColumn(
                name: "LateFeesPaid",
                table: "RentalTransaction");

            migrationBuilder.RenameTable(
                name: "RentalTransaction",
                newName: "RentalTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_RentalTransaction_CustomerId",
                table: "RentalTransactions",
                newName: "IX_RentalTransactions_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentalTransactions",
                table: "RentalTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalTransactionItems_RentalTransactions_TransactionId",
                table: "RentalTransactionItems",
                column: "TransactionId",
                principalTable: "RentalTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalTransactions_Customers_CustomerId",
                table: "RentalTransactions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
