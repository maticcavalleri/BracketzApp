using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Team owner")]
        public virtual IdentityUser IdentityUser { get; set; }
        
        //public virtual IEnumerable<BracketTeam> BracketTeam { get; set; }
        public virtual IEnumerable<ParticipantTeam> ParticipantTeam { get; set; }
        public virtual IEnumerable<TournamentTeam> TournamentTeam { get; set; }
    }
}