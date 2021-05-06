using Microsoft.EntityFrameworkCore.Migrations;

namespace Rocky.Migrations
{
    public partial class addFirstWorkToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_WorkId",
                table: "Products",
                column: "WorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FirtsWorks_WorkId",
                table: "Products",
                column: "WorkId",
                principalTable: "FirtsWorks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_FirtsWorks_WorkId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WorkId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WorkId",
                table: "Products");
        }
    }
}
