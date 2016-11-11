using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pubstars2.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    gameId = table.Column<string>(nullable: false),
                    blueScore = table.Column<int>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    redScore = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.gameId);
                });

            migrationBuilder.CreateTable(
                name: "PubstarsPlayer",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    Goals = table.Column<int>(nullable: false),
                    PubstarsGamegameId = table.Column<string>(nullable: true),
                    team = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PubstarsPlayer", x => x.Name);
                    table.ForeignKey(
                        name: "FK_PubstarsPlayer_Games_PubstarsGamegameId",
                        column: x => x.PubstarsGamegameId,
                        principalTable: "Games",
                        principalColumn: "gameId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PubstarsPlayer_PubstarsGamegameId",
                table: "PubstarsPlayer",
                column: "PubstarsGamegameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PubstarsPlayer");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
