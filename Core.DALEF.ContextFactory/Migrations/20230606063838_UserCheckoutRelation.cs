using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DALEF.ContextFactory.Migrations
{
    public partial class UserCheckoutRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TECheckout_TEMember_MemberID",
                table: "TECheckout");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "TECheckout",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_TECheckout_MemberID",
                table: "TECheckout",
                newName: "IX_TECheckout_UserID");

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("a4455c52-cd74-4230-b647-30c717f4d164"),
                column: "SecurityStamp",
                value: "266427c6-0300-4f54-a465-0f1d49a459ee");

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ddcb65c5-3170-48be-bc9a-7bc89f741286"),
                column: "SecurityStamp",
                value: "5cf8476b-6eab-4a22-a6dd-5f19550b9f60");

            migrationBuilder.AddForeignKey(
                name: "FK_TECheckout_User_UserID",
                table: "TECheckout",
                column: "UserID",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TECheckout_User_UserID",
                table: "TECheckout");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "TECheckout",
                newName: "MemberID");

            migrationBuilder.RenameIndex(
                name: "IX_TECheckout_UserID",
                table: "TECheckout",
                newName: "IX_TECheckout_MemberID");

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("a4455c52-cd74-4230-b647-30c717f4d164"),
                column: "SecurityStamp",
                value: "b382fc2f-847a-4175-a399-ec9dd01942b6");

            migrationBuilder.UpdateData(
                schema: "Identity",
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ddcb65c5-3170-48be-bc9a-7bc89f741286"),
                column: "SecurityStamp",
                value: "4f062a0f-2b9d-42f3-bb6f-f3817be01c06");

            migrationBuilder.AddForeignKey(
                name: "FK_TECheckout_TEMember_MemberID",
                table: "TECheckout",
                column: "MemberID",
                principalTable: "TEMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
