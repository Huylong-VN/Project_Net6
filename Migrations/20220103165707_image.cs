using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Management_Student.Backend.Migrations
{
    public partial class image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "bc4fedba-7219-4d53-80f1-6ea104ccecce");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "02c8965a-ac5d-473c-8c57-f2d46713c5d7", "AQAAAAEAACcQAAAAEICixlDEfs3I0OmK0727R+QM3k6cli4uY1Y5U6gNVEmb8Squ6k/qUgQcK+86kzwTJQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "ed1c3471-33ca-429f-8cfc-8e76cb00db22");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "385499ae-db21-4a36-8719-ab8b78fa2af7", "AQAAAAEAACcQAAAAEBwictHGpImkOKSdA1eTDEqrXb8Dox0KUN7tHbKhhJ4eASxVv5v1N5xI3s51S4qMNA==" });
        }
    }
}
