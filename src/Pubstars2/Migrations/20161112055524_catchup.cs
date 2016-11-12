using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pubstars2.Migrations
{
    public partial class catchup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PubstarsPlayer",
                table: "PubstarsPlayer");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PubstarsPlayer");

            migrationBuilder.AddColumn<string>(
                name: "PubstarsPlayerId",
                table: "PubstarsPlayer",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PubstarsPlayer",
                table: "PubstarsPlayer",
                column: "PubstarsPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PubstarsPlayer_PubstarsPlayerId",
                table: "PubstarsPlayer",
                column: "PubstarsPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PubstarsPlayer_AspNetUsers_PubstarsPlayerId",
                table: "PubstarsPlayer",
                column: "PubstarsPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameColumn(
                name: "team",
                table: "PubstarsPlayer",
                newName: "Team");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PubstarsPlayer_AspNetUsers_PubstarsPlayerId",
                table: "PubstarsPlayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PubstarsPlayer",
                table: "PubstarsPlayer");

            migrationBuilder.DropIndex(
                name: "IX_PubstarsPlayer_PubstarsPlayerId",
                table: "PubstarsPlayer");

            migrationBuilder.DropColumn(
                name: "PubstarsPlayerId",
                table: "PubstarsPlayer");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PubstarsPlayer",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PubstarsPlayer",
                table: "PubstarsPlayer",
                column: "Name");

            migrationBuilder.RenameColumn(
                name: "Team",
                table: "PubstarsPlayer",
                newName: "team");
        }
    }
}
