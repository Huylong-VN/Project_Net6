using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM_Management_Student.Backend.Migrations
{
    public partial class newRoleStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "154aa0b6-0d8d-435b-b12e-c7028fe0556d");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("5e54e16d-681f-4388-bf83-5cc4a57c29cd"), "794f31b2-007c-4c64-8962-2ae355f3fab5", "student", "student" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "42374f1e-30c0-4661-ad52-f35ba55a689c", "AQAAAAEAACcQAAAAEEBhPzSMqihnaStzjMVCSLX0QnVT0FmaA9JrE4/mmyl4g7x+HV+4h8aKhrhRMEXhBQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5e54e16d-681f-4388-bf83-5cc4a57c29cd"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                column: "ConcurrencyStamp",
                value: "974012ed-4975-4f9b-ab89-9bb062252598");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "41e2c8cf-96e5-46a4-9c1b-c7914cb0c131", "AQAAAAEAACcQAAAAEH0vlyLCkIkzjdwrm7NFX+i86m2HIxmeZutiaCnNV+dZ4a/V1KTij/gSIxtTbd9lkA==" });
        }
    }
}
