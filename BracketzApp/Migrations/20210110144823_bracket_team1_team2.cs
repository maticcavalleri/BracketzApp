using Microsoft.EntityFrameworkCore.Migrations;

namespace BracketzApp.Migrations
{
    public partial class bracket_team1_team2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BracketTeam");

            migrationBuilder.AddColumn<int>(
                name: "Team1Id",
                table: "Bracket",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Team2Id",
                table: "Bracket",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bracket_Team1Id",
                table: "Bracket",
                column: "Team1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bracket_Team2Id",
                table: "Bracket",
                column: "Team2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bracket_Team_Team1Id",
                table: "Bracket",
                column: "Team1Id",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bracket_Team_Team2Id",
                table: "Bracket",
                column: "Team2Id",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bracket_Team_Team1Id",
                table: "Bracket");

            migrationBuilder.DropForeignKey(
                name: "FK_Bracket_Team_Team2Id",
                table: "Bracket");

            migrationBuilder.DropIndex(
                name: "IX_Bracket_Team1Id",
                table: "Bracket");

            migrationBuilder.DropIndex(
                name: "IX_Bracket_Team2Id",
                table: "Bracket");

            migrationBuilder.DropColumn(
                name: "Team1Id",
                table: "Bracket");

            migrationBuilder.DropColumn(
                name: "Team2Id",
                table: "Bracket");

            migrationBuilder.CreateTable(
                name: "BracketTeam",
                columns: table => new
                {
                    BracketId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BracketTeam", x => new { x.BracketId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_BracketTeam_Bracket_BracketId",
                        column: x => x.BracketId,
                        principalTable: "Bracket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BracketTeam_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BracketTeam_TeamId",
                table: "BracketTeam",
                column: "TeamId");
        }
    }
}
