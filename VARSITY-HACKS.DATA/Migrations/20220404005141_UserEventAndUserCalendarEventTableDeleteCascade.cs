using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VARSITY_HACKS.DATA.Migrations
{
    public partial class UserEventAndUserCalendarEventTableDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCalendarEvent_UserEvent",
                table: "UserCalendarEvent");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCalendarEvent_UserEvent",
                table: "UserCalendarEvent",
                column: "UserEventId",
                principalTable: "UserEvent",
                principalColumn: "UserEventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCalendarEvent_UserEvent",
                table: "UserCalendarEvent");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCalendarEvent_UserEvent",
                table: "UserCalendarEvent",
                column: "UserEventId",
                principalTable: "UserEvent",
                principalColumn: "UserEventId");
        }
    }
}
