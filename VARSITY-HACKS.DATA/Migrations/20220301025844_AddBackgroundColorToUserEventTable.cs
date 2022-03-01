using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VARSITY_HACKS.DATA.Migrations
{
    public partial class AddBackgroundColorToUserEventTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "UserEvent",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "UserEvent");
        }
    }
}
