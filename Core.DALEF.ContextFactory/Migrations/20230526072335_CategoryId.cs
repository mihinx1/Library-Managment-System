using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DALEF.ContextFactory.Migrations
{
    public partial class CategoryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryID",
                table: "TEBook",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TEBook_CategoryID",
                table: "TEBook",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_TEBook_TECategory_CategoryID",
                table: "TEBook",
                column: "CategoryID",
                principalTable: "TECategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TEBook_TECategory_CategoryID",
                table: "TEBook");

            migrationBuilder.DropIndex(
                name: "IX_TEBook_CategoryID",
                table: "TEBook");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "TEBook");
        }
    }
}
