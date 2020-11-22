using System.ComponentModel.DataAnnotations.Schema;

namespace BracketzApp.Models
{
    public class TournamentFormat
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
    }
}