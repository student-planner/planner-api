using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.API.Migrations
{
    /// <inheritdoc />
    public partial class FixScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("56de3965-0e9c-4aff-b918-8375e153d747"));

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Goals",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Labor",
                table: "Goals",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "Goals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "DependGoalsIds",
                table: "Goals",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "SubGoalsIds",
                table: "Goals",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("533f6305-3035-45dc-ab3a-2598a93db2e0"), new DateTime(2023, 9, 21, 11, 4, 59, 505, DateTimeKind.Utc).AddTicks(4830), "seljmov@list.ru", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("533f6305-3035-45dc-ab3a-2598a93db2e0"));

            migrationBuilder.DropColumn(
                name: "DependGoalsIds",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "SubGoalsIds",
                table: "Goals");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Goals",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Labor",
                table: "Goals",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "Goals",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("56de3965-0e9c-4aff-b918-8375e153d747"), new DateTime(2023, 9, 20, 17, 14, 18, 848, DateTimeKind.Utc).AddTicks(7310), "seljmov@list.ru", null, null });
        }
    }
}
