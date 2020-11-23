using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BracketzApp.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        public virtual IEnumerable<BracketTeam> BracketTeam { get; set; }
    }
}