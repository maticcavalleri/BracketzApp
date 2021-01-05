using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BracketzApp.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        public string OwnerId { get; set; }
        
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser IdentityUser { get; set; }
        
        public int? TournamentId { get; set; }
        
        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; }
        
        public virtual IEnumerable<BracketTeam> BracketTeam { get; set; }
    }
}