using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BracketzApp.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int NOfGames { get; set; }

        public string Game { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("TournamentFormat")]
        public TournamentFormat TournamentFormat { get; set; }
    }
}