using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VARSITY_HACKS.DATA.Migrations
{
    public partial class AddUserEventTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEvent",
                columns: table => new
                {
                    UserEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false, computedColumnSql: "(DATEADD(MINUTE, DurationMinute, StartTime))", stored: true),
                    DurationMinute = table.Column<int>(type: "int", nullable: false),
                    DifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    PriorityLevelId = table.Column<int>(type: "int", nullable: false),
                    IsSimultaneous = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvent", x => x.UserEventId);
                    table.ForeignKey(
                        name: "FK_UserEvent_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId");
                });

            migrationBuilder.CreateTable(
                name: "UserCalendarEvent",
                columns: table => new
                {
                    UserCalendarEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEventId = table.Column<int>(type: "int", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    EventDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime", nullable: false, computedColumnSql: "(CAST(EventDate AS DATETIME) + CAST(StartTime AS DATETIME))", stored: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime", nullable: false, computedColumnSql: "(CAST(EventDate AS DATETIME) + CAST(EndTime AS DATETIME))", stored: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    PriorityLevelId = table.Column<int>(type: "int", nullable: false),
                    IsSimultaneous = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCalendarEvent", x => x.UserCalendarEventId);
                    table.ForeignKey(
                        name: "FK_UserCalendarEvent_Registration",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId");
                    table.ForeignKey(
                        name: "FK_UserCalendarEvent_UserEvent",
                        column: x => x.UserEventId,
                        principalTable: "UserEvent",
                        principalColumn: "UserEventId");
                });

            migrationBuilder.CreateTable(
                name: "UserEventDay",
                columns: table => new
                {
                    UserEventDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEventId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEventDay", x => x.UserEventDayId);
                    table.ForeignKey(
                        name: "FK_UserEventDay_UserEvent",
                        column: x => x.UserEventId,
                        principalTable: "UserEvent",
                        principalColumn: "UserEventId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendarEvent_RegistrationId",
                table: "UserCalendarEvent",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCalendarEvent_UserEventId",
                table: "UserCalendarEvent",
                column: "UserEventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvent_RegistrationId",
                table: "UserEvent",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEventDay_UserEventId",
                table: "UserEventDay",
                column: "UserEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCalendarEvent");

            migrationBuilder.DropTable(
                name: "UserEventDay");

            migrationBuilder.DropTable(
                name: "UserEvent");
        }
    }
}
