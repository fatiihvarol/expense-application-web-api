using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class CategoryAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "VpExpenses");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "VpExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VpExpenseCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VpExpenseCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VpExpenses_CategoryId",
                table: "VpExpenses",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_VpExpenses_VpExpenseCategories_CategoryId",
                table: "VpExpenses",
                column: "CategoryId",
                principalTable: "VpExpenseCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VpExpenses_VpExpenseCategories_CategoryId",
                table: "VpExpenses");

            migrationBuilder.DropTable(
                name: "VpExpenseCategories");

            migrationBuilder.DropIndex(
                name: "IX_VpExpenses_CategoryId",
                table: "VpExpenses");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "VpExpenses");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "VpExpenses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
