using System.ComponentModel.DataAnnotations.Schema;

namespace BracketzApp.Models
{
    public class Bracket
    {
        public int Id { get; set; }

        public int ScoreTeam1 { get; set; }

        public int ScoreTeam2 { get; set; }

        [ForeignKey("Bracket")] 
        public Bracket Parent { get; set; }
    }
}