using System.ComponentModel.DataAnnotations;

namespace BracketzApp.Models
{
    public class TournamentFormat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}