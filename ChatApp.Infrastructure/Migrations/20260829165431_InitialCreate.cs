using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SentTimeAndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversationParticipant",
                columns: table => new
                {
                    ConversationID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationParticipant", x => new { x.ConversationID, x.UserID });
                    table.ForeignKey(
                        name: "FK_ConversationParticipant_Conversation_ConversationID",
                        column: x => x.ConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConversationParticipant_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SentDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SentTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    SenderID = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    ConversationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Messages_Conversation_ConversationID",
                        column: x => x.ConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationParticipant_ConversationID_UserID",
                table: "ConversationParticipant",
                columns: new[] { "ConversationID", "UserID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationParticipant_UserID",
                table: "ConversationParticipant",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationID",
                table: "Messages",
                column: "ConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderID",
                table: "Messages",
                column: "SenderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationParticipant");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
