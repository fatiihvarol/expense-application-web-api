using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseFormHistoryAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseFormHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseFormId = table.Column<int>(type: "int", nullable: false),
                    MadeBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseFormHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseFormHistories_VpExpenseForms_ExpenseFormId",
                        column: x => x.ExpenseFormId,
                        principalTable: "VpExpenseForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseFormHistories_ExpenseFormId",
                table: "ExpenseFormHistories",
                column: "ExpenseFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseFormHistories");
        }
    }
}
