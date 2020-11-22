using System.ComponentModel.DataAnnotations.Schema;

namespace BracketzApp.Models
{
    public class BracketsTeams
    {
        [ForeignKey("Bracket")]
        public Bracket Bracket { get; set; }

        [ForeignKey("Team")]
        public Team Team { get; set; }
    }
}