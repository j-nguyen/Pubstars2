using Microsoft.EntityFrameworkCore.Migrations;

namespace Pubstars2.Migrations
{
    public partial class local2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PubstarsSeasonseasonName",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_PubstarsSeasonseasonName",
                table: "Games",
                column: "PubstarsSeasonseasonName");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_PubstarsSeason_PubstarsSeasonseasonName",
                table: "Games",
                column: "PubstarsSeasonseasonName",
                principalTable: "PubstarsSeason",
                principalColumn: "seasonName",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_PubstarsSeason_PubstarsSeasonseasonName",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_PubstarsSeasonseasonName",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PubstarsSeasonseasonName",
                table: "Games");
        }
    }
}
