using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiZkteco.Migrations
{
    public partial class AddDisabledColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "activeAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "disabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activeAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "disabled",
                table: "Users");
        }
    }
}
