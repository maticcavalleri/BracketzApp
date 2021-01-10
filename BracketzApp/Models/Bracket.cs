using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BracketzApp.Models
{
    public class Bracket
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Team 1")]
        public int? Team1Id { get; set; }
        [ForeignKey("Team1Id")]
        public virtual Team Team1 { get; set; }

        [DisplayName("Team 2")]
        public int? Team2Id { get; set; }
        [ForeignKey("Team2Id")]
        public virtual Team Team2 { get; set; }

        [Required]
        [DefaultValue(0)]
        [DisplayName("Team 1 Score")]
        public int ScoreTeam1 { get; set; }

        [Required]
        [DefaultValue(0)]
        [DisplayName("Team 2 Score")]
        public int ScoreTeam2 { get; set; }

        [Required] 
        public int Index { get; set; }

        [Required]
        public bool IsFinished { get; set; }

        public int? TournamentId { get; set; }
        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; }
        
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")] 
        public virtual Bracket Parent { get; set; }

        //public virtual IEnumerable<BracketTeam> BracketTeam { get; set; }
    }
}