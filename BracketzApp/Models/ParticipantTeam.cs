using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using BracketzApp.Models;

public class ParticipantTeam
{
    public int ParticipantId { get; set; } //foreign key property
    [ForeignKey("ParticipantId")]
    public Participant Participant { get; set; } //Reference navigation property

    public int TeamId { get; set; } //foreign key property
    [ForeignKey("TeamId")]
    public Team Team { get; set; } //Reference navigation property
}