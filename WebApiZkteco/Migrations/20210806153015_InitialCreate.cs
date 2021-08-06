using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiZkteco.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    sUserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    sName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    iPrivilege = table.Column<int>(type: "int", nullable: false),
                    bEnabled = table.Column<bool>(type: "bit", nullable: false),
                    idwFingerIndex = table.Column<int>(type: "int", nullable: false),
                    iFingerFlag = table.Column<int>(type: "int", nullable: false),
                    sFingerData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    iFingerLen = table.Column<int>(type: "int", nullable: false),
                    iFaceIndex = table.Column<int>(type: "int", nullable: false),
                    sFaceData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    iFaceLen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.sUserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
