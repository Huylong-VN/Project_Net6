using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Management_Student.Backend.Migrations
{
    public partial class updaterelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClasses_Users_AppUserId",
                table: "UserClasses");

            migrationBuilder.DropIndex(
                name: "IX_UserClasses_AppUserId",
                table: "UserClasses");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserClasses");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "726a580c-de6a-4b95-bd20-15efdb508f4d");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b4eefc97-8253-4af3-a8b5-c083a9f7bc12", "AQAAAAEAACcQAAAAEAZaoITZLKX6qrLFujMBexywmt4hmhq9/KYOW0ta595Mnyjr9wjh2IbKaLyIryKG7g==" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserClasses_Users_UserId",
                table: "UserClasses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClasses_Users_UserId",
                table: "UserClasses");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "UserClasses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "de5fe0a7-8c86-48c2-bb32-0070204c4b9e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9879b9a0-ee5f-4a4f-8788-cfc938b5b27e", "AQAAAAEAACcQAAAAEA9qJ4ohNEvnyXAmcdi37w23gG7YGZ20hxjpPST58Mdr3Aj3peOdLSdbAUloBX+U/g==" });

            migrationBuilder.CreateIndex(
                name: "IX_UserClasses_AppUserId",
                table: "UserClasses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClasses_Users_AppUserId",
                table: "UserClasses",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
