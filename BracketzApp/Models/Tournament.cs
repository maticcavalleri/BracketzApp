using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BracketzApp.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name of the Tournament")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Best of")]
        public int NOfGames { get; set; }

        [Required]
        public string Game { get; set; }

        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Tournament Format")]
        public int TournamentFormatId { get; set; }
        
        [DisplayName("Tournament Format")]
        [ForeignKey("TournamentFormatId")]
        public virtual TournamentFormat TournamentFormat { get; set; }

        public virtual IEnumerable<TournamentTeam> TournamentTeam { get; set; }
    }
}