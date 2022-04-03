using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VARSITY_HACKS.DATA.Migrations
{
    public partial class AddDurationMinuteInUserCalendarEventTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinute",
                table: "UserCalendarEvent",
                type: "int",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTime",
                table: "UserCalendarEvent",
                type: "datetime",
                nullable: false,
                computedColumnSql: "(DATEADD(MINUTE, [DurationMinute], (CONVERT([datetime],[EventDate])+CONVERT([datetime],[StartTime]))))",
                stored: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldComputedColumnSql: "(CAST(EventDate AS DATETIME) + CAST(EndTime AS DATETIME))",
                oldStored: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "UserCalendarEvent",
                type: "time",
                nullable: false,
                computedColumnSql: "(DATEADD(MINUTE, DurationMinute, StartTime))",
                stored: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "UserCalendarEvent",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldComputedColumnSql: "(DATEADD(MINUTE, DurationMinute, StartTime))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDateTime",
                table: "UserCalendarEvent",
                type: "datetime",
                nullable: false,
                computedColumnSql: "(CAST(EventDate AS DATETIME) + CAST(EndTime AS DATETIME))",
                stored: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldComputedColumnSql: "(DATEADD(MINUTE, [DurationMinute], (CONVERT([datetime],[EventDate])+CONVERT([datetime],[StartTime]))))",
                oldStored: true);

            migrationBuilder.DropColumn(
                name: "DurationMinute",
                table: "UserCalendarEvent");
        }
    }
}
