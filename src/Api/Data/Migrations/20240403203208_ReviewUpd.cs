using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReviewUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                columns: new[] { "UserId", "ProductId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                column: "UserId");
        }
    }
}
