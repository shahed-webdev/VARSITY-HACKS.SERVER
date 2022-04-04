using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VARSITY_HACKS.DATA.Migrations
{
    public partial class UserEventAndUserEventDayTableDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEventDay_UserEvent",
                table: "UserEventDay");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEventDay_UserEvent",
                table: "UserEventDay",
                column: "UserEventId",
                principalTable: "UserEvent",
                principalColumn: "UserEventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEventDay_UserEvent",
                table: "UserEventDay");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEventDay_UserEvent",
                table: "UserEventDay",
                column: "UserEventId",
                principalTable: "UserEvent",
                principalColumn: "UserEventId");
        }
    }
}
