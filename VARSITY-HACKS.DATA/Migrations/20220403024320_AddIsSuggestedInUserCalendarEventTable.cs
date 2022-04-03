using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VARSITY_HACKS.DATA.Migrations
{
    public partial class AddIsSuggestedInUserCalendarEventTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuggested",
                table: "UserCalendarEvent",
                type: "bit",
                nullable: false,
                defaultValueSql: "((0))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuggested",
                table: "UserCalendarEvent");
        }
    }
}
