using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DALEF.ContextFactory.Migrations
{
    public partial class CategoryColorChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryColor",
                table: "TEBook");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "TECategory",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "TECategory");

            migrationBuilder.AddColumn<string>(
                name: "CategoryColor",
                table: "TEBook",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
