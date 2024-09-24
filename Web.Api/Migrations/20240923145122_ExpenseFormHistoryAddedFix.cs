using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseFormHistoryAddedFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseFormHistories_VpExpenseForms_ExpenseFormId",
                table: "ExpenseFormHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseFormHistories",
                table: "ExpenseFormHistories");

            migrationBuilder.RenameTable(
                name: "ExpenseFormHistories",
                newName: "VpExpenseFormHistories");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseFormHistories_ExpenseFormId",
                table: "VpExpenseFormHistories",
                newName: "IX_VpExpenseFormHistories_ExpenseFormId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VpExpenseFormHistories",
                table: "VpExpenseFormHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VpExpenseFormHistories_VpExpenseForms_ExpenseFormId",
                table: "VpExpenseFormHistories",
                column: "ExpenseFormId",
                principalTable: "VpExpenseForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VpExpenseFormHistories_VpExpenseForms_ExpenseFormId",
                table: "VpExpenseFormHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VpExpenseFormHistories",
                table: "VpExpenseFormHistories");

            migrationBuilder.RenameTable(
                name: "VpExpenseFormHistories",
                newName: "ExpenseFormHistories");

            migrationBuilder.RenameIndex(
                name: "IX_VpExpenseFormHistories_ExpenseFormId",
                table: "ExpenseFormHistories",
                newName: "IX_ExpenseFormHistories_ExpenseFormId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseFormHistories",
                table: "ExpenseFormHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseFormHistories_VpExpenseForms_ExpenseFormId",
                table: "ExpenseFormHistories",
                column: "ExpenseFormId",
                principalTable: "VpExpenseForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
