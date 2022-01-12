using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Management_Student.Backend.Migrations
{
    public partial class notify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserModified = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "ea702bf2-6467-4538-8abb-57014fb20fe4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Status" },
                values: new object[] { "a271964e-a7bd-434b-8b1c-f81c832b9ca4", "AQAAAAEAACcQAAAAEOY1bnxLQ/p9kgi8TfHCd8xptdGvcje3CI0KqmoAhy5CgwkIwTN1jl8iWutvfvyYQg==", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "Status" },
                values: new object[] { "ac78be5d-7e54-4a97-bfd6-4fec6a46b5bc", "AQAAAAEAACcQAAAAEElR2Ku0ZCrQoCnniZrJ+rvn8qNtNYiYhC44b6xglwgnolHhLU8qHBOsFw8SQfPChg==", null });
        }
    }
}
