using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BracketzApp.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NOfGames { get; set; }

        public string Game { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("TournamentFormat")]
        public TournamentFormat TournamentFormat { get; set; }
        
        [ForeignKey("Bracket")]
        public Bracket[] Brackets { get; set; }
    }
}