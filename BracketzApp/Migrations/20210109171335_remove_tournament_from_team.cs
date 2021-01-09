using Microsoft.EntityFrameworkCore.Migrations;

namespace BracketzApp.Migrations
{
    public partial class remove_tournament_from_team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Tournament_TournamentId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_TournamentId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Team");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentId",
                table: "Team",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_TournamentId",
                table: "Team",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Tournament_TournamentId",
                table: "Team",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
