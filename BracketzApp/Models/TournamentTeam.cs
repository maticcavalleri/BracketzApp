using System.ComponentModel.DataAnnotations.Schema;
using BracketzApp.Models;

public class TournamentTeam
{
    public int TournamentId { get; set; } //foreign key property
    [ForeignKey("TournamentId")]
    public Tournament Tournament { get; set; } //Reference navigation property

    public int TeamId { get; set; } //foreign key property
    [ForeignKey("TeamId")]
    public Team Team { get; set; } //Reference navigation property
}