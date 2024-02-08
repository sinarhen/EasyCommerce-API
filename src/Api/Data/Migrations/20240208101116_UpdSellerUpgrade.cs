using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdSellerUpgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SellerInfoId",
                table: "SellerUpgradeRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SellerUpgradeRequests_SellerInfoId",
                table: "SellerUpgradeRequests",
                column: "SellerInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerUpgradeRequests_Sellers_SellerInfoId",
                table: "SellerUpgradeRequests",
                column: "SellerInfoId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerUpgradeRequests_Sellers_SellerInfoId",
                table: "SellerUpgradeRequests");

            migrationBuilder.DropIndex(
                name: "IX_SellerUpgradeRequests_SellerInfoId",
                table: "SellerUpgradeRequests");

            migrationBuilder.DropColumn(
                name: "SellerInfoId",
                table: "SellerUpgradeRequests");
        }
    }
}
