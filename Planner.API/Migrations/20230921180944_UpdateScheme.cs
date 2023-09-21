using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("533f6305-3035-45dc-ab3a-2598a93db2e0"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "DeviceDescription",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Goals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "DeviceDescription", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("32e5076f-9e47-4ab6-87fc-aebcc24e7209"), new DateTime(2023, 9, 21, 18, 9, 44, 330, DateTimeKind.Utc).AddTicks(3210), null, "seljmov@list.ru", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("32e5076f-9e47-4ab6-87fc-aebcc24e7209"));

            migrationBuilder.DropColumn(
                name: "DeviceDescription",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Goals");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("533f6305-3035-45dc-ab3a-2598a93db2e0"), new DateTime(2023, 9, 21, 11, 4, 59, 505, DateTimeKind.Utc).AddTicks(4830), "seljmov@list.ru", null, null });
        }
    }
}
