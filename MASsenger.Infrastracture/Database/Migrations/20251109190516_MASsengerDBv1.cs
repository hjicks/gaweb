using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASsenger.Infrastracture.Database.Migrations
{
    public partial class MASsengerDBv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsVerified = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseUsers_BaseUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseUsers_BaseUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BaseChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseChats_BaseUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BotUser",
                columns: table => new
                {
                    BotsJoinedId = table.Column<int>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUser", x => new { x.BotsJoinedId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BotUser_BaseUsers_BotsJoinedId",
                        column: x => x.BotsJoinedId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BotUser_BaseUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DestinationId = table.Column<int>(type: "INTEGER", nullable: false),
                    SentTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseMessages_BaseChats_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "BaseChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseMessages_BaseUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserChannelChat",
                columns: table => new
                {
                    ChannelsJoinedId = table.Column<int>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChannelChat", x => new { x.ChannelsJoinedId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BaseUserChannelChat_BaseChats_ChannelsJoinedId",
                        column: x => x.ChannelsJoinedId,
                        principalTable: "BaseChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChannelChat_BaseUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserChannelChat1",
                columns: table => new
                {
                    AdminsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelsManagedId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChannelChat1", x => new { x.AdminsId, x.ChannelsManagedId });
                    table.ForeignKey(
                        name: "FK_BaseUserChannelChat1_BaseChats_ChannelsManagedId",
                        column: x => x.ChannelsManagedId,
                        principalTable: "BaseChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChannelChat1_BaseUsers_AdminsId",
                        column: x => x.AdminsId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserChannelChat2",
                columns: table => new
                {
                    BannedId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChannelsBannedFromId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChannelChat2", x => new { x.BannedId, x.ChannelsBannedFromId });
                    table.ForeignKey(
                        name: "FK_BaseUserChannelChat2_BaseChats_ChannelsBannedFromId",
                        column: x => x.ChannelsBannedFromId,
                        principalTable: "BaseChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChannelChat2_BaseUsers_BannedId",
                        column: x => x.BannedId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseChats_OwnerId",
                table: "BaseChats",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_DestinationId",
                table: "BaseMessages",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_SenderId",
                table: "BaseMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChannelChat_MembersId",
                table: "BaseUserChannelChat",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChannelChat1_ChannelsManagedId",
                table: "BaseUserChannelChat1",
                column: "ChannelsManagedId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChannelChat2_ChannelsBannedFromId",
                table: "BaseUserChannelChat2",
                column: "ChannelsBannedFromId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUsers_OwnerId",
                table: "BaseUsers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUsers_UserId",
                table: "BaseUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BotUser_MembersId",
                table: "BotUser",
                column: "MembersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseMessages");

            migrationBuilder.DropTable(
                name: "BaseUserChannelChat");

            migrationBuilder.DropTable(
                name: "BaseUserChannelChat1");

            migrationBuilder.DropTable(
                name: "BaseUserChannelChat2");

            migrationBuilder.DropTable(
                name: "BotUser");

            migrationBuilder.DropTable(
                name: "BaseChats");

            migrationBuilder.DropTable(
                name: "BaseUsers");
        }
    }
}
