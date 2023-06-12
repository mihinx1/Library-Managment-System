using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DALEF.ContextFactory.Migrations
{
    public partial class CheckoutFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookReturned",
                table: "TECheckout");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "TECheckout",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "TECheckout",
                newName: "PlanedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TECheckout",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TECheckout");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "TECheckout",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "PlanedDate",
                table: "TECheckout",
                newName: "EndTime");

            migrationBuilder.AddColumn<bool>(
                name: "BookReturned",
                table: "TECheckout",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
