using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DALEF.ContextFactory.Migrations
{
    public partial class BookReturned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "TEBookCopy");

            migrationBuilder.AddColumn<bool>(
                name: "BookReturned",
                table: "TECheckout",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookReturned",
                table: "TECheckout");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "TEBookCopy",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
