using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASsenger.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectChats",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseMsg",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    DirectChatId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    GroupId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMsg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseMsg_DirectChats_DirectChatId",
                        column: x => x.DirectChatId,
                        principalTable: "DirectChats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BaseUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsVerified = table.Column<bool>(type: "INTEGER", nullable: false),
                    ChannelId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    DirectChatId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    GroupId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseUser_BaseUser_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BaseUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseUser_BaseUser_UserId",
                        column: x => x.UserId,
                        principalTable: "BaseUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseUser_DirectChats_DirectChatId",
                        column: x => x.DirectChatId,
                        principalTable: "DirectChats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BotUser",
                columns: table => new
                {
                    BotsId = table.Column<int>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUser", x => new { x.BotsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BotUser_BaseUser_BotsId",
                        column: x => x.BotsId,
                        principalTable: "BaseUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BotUser_BaseUser_MembersId",
                        column: x => x.MembersId,
                        principalTable: "BaseUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_BaseUser_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BaseUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_BaseUser_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "BaseUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserChannel",
                columns: table => new
                {
                    ChannelsId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChannel", x => new { x.ChannelsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BaseUserChannel_BaseUser_MembersId",
                        column: x => x.MembersId,
                        principalTable: "BaseUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChannel_Channels_ChannelsId",
                        column: x => x.ChannelsId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserGroup",
                columns: table => new
                {
                    GroupsId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserGroup", x => new { x.GroupsId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BaseUserGroup_BaseUser_MembersId",
                        column: x => x.MembersId,
                        principalTable: "BaseUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserGroup_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseMsg_ChannelId",
                table: "BaseMsg",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMsg_DirectChatId",
                table: "BaseMsg",
                column: "DirectChatId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMsg_GroupId",
                table: "BaseMsg",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUser_ChannelId",
                table: "BaseUser",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUser_DirectChatId",
                table: "BaseUser",
                column: "DirectChatId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUser_GroupId",
                table: "BaseUser",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUser_OwnerId",
                table: "BaseUser",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUser_UserId",
                table: "BaseUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChannel_MembersId",
                table: "BaseUserChannel",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserGroup_MembersId",
                table: "BaseUserGroup",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_BotUser_MembersId",
                table: "BotUser",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_OwnerId",
                table: "Channels",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseMsg_Channels_ChannelId",
                table: "BaseMsg",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseMsg_Groups_GroupId",
                table: "BaseMsg",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseUser_Channels_ChannelId",
                table: "BaseUser",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseUser_Groups_GroupId",
                table: "BaseUser",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseUser_Channels_ChannelId",
                table: "BaseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseUser_DirectChats_DirectChatId",
                table: "BaseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseUser_Groups_GroupId",
                table: "BaseUser");

            migrationBuilder.DropTable(
                name: "BaseMsg");

            migrationBuilder.DropTable(
                name: "BaseUserChannel");

            migrationBuilder.DropTable(
                name: "BaseUserGroup");

            migrationBuilder.DropTable(
                name: "BotUser");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "DirectChats");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "BaseUser");
        }
    }
}
