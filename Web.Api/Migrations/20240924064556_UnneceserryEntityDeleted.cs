using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class UnneceserryEntityDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseFormLogs");

            migrationBuilder.DropTable(
                name: "VpTransactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseFormLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseFormId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NewCurrency = table.Column<int>(type: "int", nullable: true),
                    NewExpenseStatus = table.Column<int>(type: "int", nullable: true),
                    NewTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OldCurrency = table.Column<int>(type: "int", nullable: true),
                    OldExpenseStatus = table.Column<int>(type: "int", nullable: true),
                    OldTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseFormLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseFormLogs_VpExpenseForms_ExpenseFormId",
                        column: x => x.ExpenseFormId,
                        principalTable: "VpExpenseForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VpTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseFormId = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrencyEnum = table.Column<int>(type: "int", nullable: false),
                    IbanNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VpTransactions_VpExpenseForms_ExpenseFormId",
                        column: x => x.ExpenseFormId,
                        principalTable: "VpExpenseForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseFormLogs_ExpenseFormId",
                table: "ExpenseFormLogs",
                column: "ExpenseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_VpTransactions_ExpenseFormId",
                table: "VpTransactions",
                column: "ExpenseFormId",
                unique: true);
        }
    }
}
