using Microsoft.EntityFrameworkCore.Migrations;

namespace BracketzApp.Migrations
{
    public partial class remove_team_from_participant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Team_TeamId",
                table: "Participant");

            migrationBuilder.DropIndex(
                name: "IX_Participant_TeamId",
                table: "Participant");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Participant");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Bracket",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Bracket",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Bracket");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Bracket");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Participant",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Participant_TeamId",
                table: "Participant",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Team_TeamId",
                table: "Participant",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
