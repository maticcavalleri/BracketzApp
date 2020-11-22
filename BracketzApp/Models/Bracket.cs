using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BracketzApp.Models
{
    public class Bracket
    {
        [Key]
        public int Id { get; set; }

        public int ScoreTeam1 { get; set; }

        public int ScoreTeam2 { get; set; }

        [ForeignKey("Tournament")]
        public Tournament Tournament { get; set; }
        
        [ForeignKey("Bracket")] 
        public Bracket Parent { get; set; }
    }
}