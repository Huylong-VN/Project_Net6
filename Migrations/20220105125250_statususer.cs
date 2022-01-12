using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Management_Student.Backend.Migrations
{
    public partial class statususer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "d0728f9b-a95e-4078-bb69-3d11faef7527");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ac78be5d-7e54-4a97-bfd6-4fec6a46b5bc", "AQAAAAEAACcQAAAAEElR2Ku0ZCrQoCnniZrJ+rvn8qNtNYiYhC44b6xglwgnolHhLU8qHBOsFw8SQfPChg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

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
    }
}
