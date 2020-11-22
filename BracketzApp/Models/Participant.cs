using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BracketzApp.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int EloRating { get; set; }

        [ForeignKey("IdentityUser")]
        public IdentityUser RegisteredUser { get; set; }

        [ForeignKey("Team")]
        public Team Team { get; set; }
    }
}