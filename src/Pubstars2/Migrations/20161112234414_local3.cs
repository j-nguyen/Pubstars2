using Microsoft.EntityFrameworkCore.Migrations;

namespace Pubstars2.Migrations
{
    public partial class local3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "PubstarsSeason");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PubstarsSeason",
                columns: table => new
                {
                    seasonName = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PubstarsSeason", x => x.seasonName);
                    table.ForeignKey(
                        name: "FK_PubstarsSeason_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "PubstarsSeasonseasonName",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_PubstarsSeasonseasonName",
                table: "Games",
                column: "PubstarsSeasonseasonName");

            migrationBuilder.CreateIndex(
                name: "IX_PubstarsSeason_ApplicationUserId",
                table: "PubstarsSeason",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_PubstarsSeason_PubstarsSeasonseasonName",
                table: "Games",
                column: "PubstarsSeasonseasonName",
                principalTable: "PubstarsSeason",
                principalColumn: "seasonName",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
