using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BracketzApp.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Username")]
        public string Name { get; set; }

        [Required]
        [DefaultValue(1000)]
        [DisplayName("Rating")]
        public int EloRating { get; set; }
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public virtual IEnumerable<ParticipantTeam> ParticipantTeam { get; set; }
    }
}