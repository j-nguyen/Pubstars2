using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pubstars2.Migrations
{
    public partial class pw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "steamId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "psPassword",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "psPassword",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "steamId",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
