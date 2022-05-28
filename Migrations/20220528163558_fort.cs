using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class fort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_AspNetUsers_AppUserId",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role",
                table: "role");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "AppRole");

            migrationBuilder.RenameIndex(
                name: "IX_role_AppUserId",
                table: "AppRole",
                newName: "IX_AppRole_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRole",
                table: "AppRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRole_AspNetUsers_AppUserId",
                table: "AppRole",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRole_AspNetUsers_AppUserId",
                table: "AppRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRole",
                table: "AppRole");

            migrationBuilder.RenameTable(
                name: "AppRole",
                newName: "role");

            migrationBuilder.RenameIndex(
                name: "IX_AppRole_AppUserId",
                table: "role",
                newName: "IX_role_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role",
                table: "role",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_role_AspNetUsers_AppUserId",
                table: "role",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
