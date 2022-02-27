using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VARSITY_HACKS.DATA.Migrations
{
    public partial class AddRegistrationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Validation = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    PersonalityTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    State = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UniversityName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SocialMediaLink = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    InsertDateUtc = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.RegistrationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registration");
        }
    }
}
