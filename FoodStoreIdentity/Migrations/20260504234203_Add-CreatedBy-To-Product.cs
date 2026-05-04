using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStoreIdentity.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedByUserId",
                table: "Products",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CreatedByUserId",
                table: "Products",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CreatedByUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreatedByUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Products");
        }
    }
}
