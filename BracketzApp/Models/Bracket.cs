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

        [Required]
        [DefaultValue(0)]
        public int ScoreTeam1 { get; set; }

        [Required]
        [DefaultValue(0)]
        public int ScoreTeam2 { get; set; }

        public int? TournamentId { get; set; }
        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; }
        
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")] 
        public virtual Bracket Parent { get; set; }

        public virtual IEnumerable<BracketTeam> BracketTeam { get; set; }
    }
}