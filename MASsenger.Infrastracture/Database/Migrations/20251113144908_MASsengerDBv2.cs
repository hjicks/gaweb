using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASsenger.Infrastracture.Database.Migrations
{
    public partial class MASsengerDBv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "BaseUsers",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "BaseUsers",
                type: "BLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "BaseUsers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "BaseUsers");
        }
    }
}
