using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.API.Migrations
{
    /// <inheritdoc />
    public partial class editContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b05b8af-0623-4527-8df8-f06c6836efed"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Goals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("773ea00f-75dc-4f93-aaa4-7373b3db9358"), new DateTime(2023, 9, 10, 14, 58, 55, 336, DateTimeKind.Utc).AddTicks(7557), "seljmov@list.ru", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                table: "Goals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_Users_UserId",
                table: "Goals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_Users_UserId",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Goals_UserId",
                table: "Goals");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("773ea00f-75dc-4f93-aaa4-7373b3db9358"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Goals");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("3b05b8af-0623-4527-8df8-f06c6836efed"), new DateTime(2023, 9, 9, 16, 44, 47, 871, DateTimeKind.Utc).AddTicks(3160), "seljmov@list.ru", null, null });
        }
    }
}
