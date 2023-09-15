using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.API.Migrations
{
    /// <inheritdoc />
    public partial class addHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthTicket",
                table: "AuthTicket");

            migrationBuilder.RenameTable(
                name: "AuthTicket",
                newName: "AuthTickets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthTickets",
                table: "AuthTickets",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "RefreshToken", "RefreshTokenExpires" },
                values: new object[] { new Guid("3b05b8af-0623-4527-8df8-f06c6836efed"), new DateTime(2023, 9, 9, 16, 44, 47, 871, DateTimeKind.Utc).AddTicks(3160), "seljmov@list.ru", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthTickets",
                table: "AuthTickets");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b05b8af-0623-4527-8df8-f06c6836efed"));

            migrationBuilder.RenameTable(
                name: "AuthTickets",
                newName: "AuthTicket");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthTicket",
                table: "AuthTicket",
                column: "Id");
        }
    }
}
