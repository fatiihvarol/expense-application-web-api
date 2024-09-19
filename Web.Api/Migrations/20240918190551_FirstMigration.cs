using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VpApplicationUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VpApplicationUser_VpApplicationUser_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "VpApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VpExpenseForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyEnum = table.Column<int>(type: "int", nullable: false),
                    ExpenseStatusEnum = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    AccountantId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpExpenseForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VpExpenseForms_VpApplicationUser_AccountantId",
                        column: x => x.AccountantId,
                        principalTable: "VpApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VpExpenseForms_VpApplicationUser_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "VpApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VpExpenseForms_VpApplicationUser_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "VpApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VpExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseFormId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VpExpenses_VpExpenseForms_ExpenseFormId",
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
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IbanNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyEnum = table.Column<int>(type: "int", nullable: false),
                    ExpenseFormId = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                name: "IX_VpApplicationUser_ManagerId",
                table: "VpApplicationUser",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_VpExpenseForms_AccountantId",
                table: "VpExpenseForms",
                column: "AccountantId");

            migrationBuilder.CreateIndex(
                name: "IX_VpExpenseForms_EmployeeId",
                table: "VpExpenseForms",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_VpExpenseForms_ManagerId",
                table: "VpExpenseForms",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_VpExpenses_ExpenseFormId",
                table: "VpExpenses",
                column: "ExpenseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_VpTransactions_ExpenseFormId",
                table: "VpTransactions",
                column: "ExpenseFormId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VpExpenses");

            migrationBuilder.DropTable(
                name: "VpTransactions");

            migrationBuilder.DropTable(
                name: "VpExpenseForms");

            migrationBuilder.DropTable(
                name: "VpApplicationUser");
        }
    }
}
