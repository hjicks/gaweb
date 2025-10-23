using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MASsenger.Infrastracture.Migrations
{
    public partial class MASsengerDBv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseChats",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    LinkedChannelGroupId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    OwnerId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseChats_BaseChats_LinkedChannelGroupId",
                        column: x => x.LinkedChannelGroupId,
                        principalTable: "BaseChats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BaseMessages",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    DestinationId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    SentTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeleterId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "BaseUsers",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsVerified = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: true),
                    OwnerId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    BaseMessageId = table.Column<ulong>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseUsers_BaseMessages_BaseMessageId",
                        column: x => x.BaseMessageId,
                        principalTable: "BaseMessages",
                        principalColumn: "Id");
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
                name: "BaseUserChannelGroupChat",
                columns: table => new
                {
                    ChannelGroupsJoinedId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChannelGroupChat", x => new { x.ChannelGroupsJoinedId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_BaseUserChannelGroupChat_BaseChats_ChannelGroupsJoinedId",
                        column: x => x.ChannelGroupsJoinedId,
                        principalTable: "BaseChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChannelGroupChat_BaseUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserChannelGroupChat1",
                columns: table => new
                {
                    AdminsId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ChannelGroupsManagedId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChannelGroupChat1", x => new { x.AdminsId, x.ChannelGroupsManagedId });
                    table.ForeignKey(
                        name: "FK_BaseUserChannelGroupChat1_BaseChats_ChannelGroupsManagedId",
                        column: x => x.ChannelGroupsManagedId,
                        principalTable: "BaseChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChannelGroupChat1_BaseUsers_AdminsId",
                        column: x => x.AdminsId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseUserChannelGroupChat2",
                columns: table => new
                {
                    BannedId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ChannelGroupsBannedFromId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseUserChannelGroupChat2", x => new { x.BannedId, x.ChannelGroupsBannedFromId });
                    table.ForeignKey(
                        name: "FK_BaseUserChannelGroupChat2_BaseChats_ChannelGroupsBannedFromId",
                        column: x => x.ChannelGroupsBannedFromId,
                        principalTable: "BaseChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseUserChannelGroupChat2_BaseUsers_BannedId",
                        column: x => x.BannedId,
                        principalTable: "BaseUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BotUser",
                columns: table => new
                {
                    BotsJoinedId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MembersId = table.Column<ulong>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_BaseChats_LinkedChannelGroupId",
                table: "BaseChats",
                column: "LinkedChannelGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseChats_OwnerId",
                table: "BaseChats",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_DeleterId",
                table: "BaseMessages",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_DestinationId",
                table: "BaseMessages",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_SenderId",
                table: "BaseMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChannelGroupChat_MembersId",
                table: "BaseUserChannelGroupChat",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChannelGroupChat1_ChannelGroupsManagedId",
                table: "BaseUserChannelGroupChat1",
                column: "ChannelGroupsManagedId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUserChannelGroupChat2_ChannelGroupsBannedFromId",
                table: "BaseUserChannelGroupChat2",
                column: "ChannelGroupsBannedFromId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseUsers_BaseMessageId",
                table: "BaseUsers",
                column: "BaseMessageId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BaseChats_BaseUsers_OwnerId",
                table: "BaseChats",
                column: "OwnerId",
                principalTable: "BaseUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseMessages_BaseUsers_DeleterId",
                table: "BaseMessages",
                column: "DeleterId",
                principalTable: "BaseUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseMessages_BaseUsers_SenderId",
                table: "BaseMessages",
                column: "SenderId",
                principalTable: "BaseUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseChats_BaseUsers_OwnerId",
                table: "BaseChats");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseMessages_BaseUsers_DeleterId",
                table: "BaseMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseMessages_BaseUsers_SenderId",
                table: "BaseMessages");

            migrationBuilder.DropTable(
                name: "BaseUserChannelGroupChat");

            migrationBuilder.DropTable(
                name: "BaseUserChannelGroupChat1");

            migrationBuilder.DropTable(
                name: "BaseUserChannelGroupChat2");

            migrationBuilder.DropTable(
                name: "BotUser");

            migrationBuilder.DropTable(
                name: "BaseUsers");

            migrationBuilder.DropTable(
                name: "BaseMessages");

            migrationBuilder.DropTable(
                name: "BaseChats");
        }
    }
}
