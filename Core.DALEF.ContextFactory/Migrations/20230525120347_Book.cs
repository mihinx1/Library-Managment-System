using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DALEF.ContextFactory.Migrations
{
    public partial class Book : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TEBook_TEPublisher_PublisherID",
                table: "TEBook");

            migrationBuilder.DropIndex(
                name: "IX_TEBook_PublisherID",
                table: "TEBook");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TEBook");

            migrationBuilder.DropColumn(
                name: "PublisherID",
                table: "TEBook");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "TEBook",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "TEBook",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TEBook",
                newName: "Genre");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "TEBook",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TEBook",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PublisherID",
                table: "TEBook",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TEBook_PublisherID",
                table: "TEBook",
                column: "PublisherID");

            migrationBuilder.AddForeignKey(
                name: "FK_TEBook_TEPublisher_PublisherID",
                table: "TEBook",
                column: "PublisherID",
                principalTable: "TEPublisher",
                principalColumn: "Id");
        }
    }
}
