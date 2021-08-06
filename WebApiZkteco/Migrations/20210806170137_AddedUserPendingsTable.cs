using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiZkteco.Migrations
{
    public partial class AddedUserPendingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPendings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usersUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    activeAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPendings", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserPendings_Users_usersUserID",
                        column: x => x.usersUserID,
                        principalTable: "Users",
                        principalColumn: "sUserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPendings_usersUserID",
                table: "UserPendings",
                column: "usersUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPendings");
        }
    }
}
