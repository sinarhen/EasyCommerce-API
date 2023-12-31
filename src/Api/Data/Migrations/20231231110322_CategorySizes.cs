using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class CategorySizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Categories_CategoryId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_CategoryId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Sizes");

            migrationBuilder.CreateTable(
                name: "CategorySizes",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    SizeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySizes", x => new { x.CategoryId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_CategorySizes_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategorySizes_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategorySizes_SizeId",
                table: "CategorySizes",
                column: "SizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategorySizes");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Sizes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_CategoryId",
                table: "Sizes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Categories_CategoryId",
                table: "Sizes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
