using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("773ea00f-75dc-4f93-aaa4-7373b3db9358"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Goals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("56de3965-0e9c-4aff-b918-8375e153d747"), new DateTime(2023, 9, 20, 17, 14, 18, 848, DateTimeKind.Utc).AddTicks(7310), "seljmov@list.ru", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("56de3965-0e9c-4aff-b918-8375e153d747"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Goals");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("773ea00f-75dc-4f93-aaa4-7373b3db9358"), new DateTime(2023, 9, 10, 14, 58, 55, 336, DateTimeKind.Utc).AddTicks(7557), "seljmov@list.ru", null, null });
        }
    }
}
