using System.ComponentModel.DataAnnotations.Schema;
using BracketzApp.Models;

public class BracketTeam
{
    public int BracketId { get; set; } //foreign key property
    [ForeignKey("BracketId")]
    public Bracket Bracket { get; set; } //Reference navigation property

    public int TeamId { get; set; } //foreign key property
    [ForeignKey("TeamId")]
    public Team Team { get; set; } //Reference navigation property
}