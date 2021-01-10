using BracketzApp.Models;

namespace BracketzApp.CustomClasses
{
    public class UsefulBracket
    {
        public int Id { get; set; }
        
        public int ScoreTeam1 { get; set; }
        
        public int ScoreTeam2 { get; set; }
        
        public int Index { get; set; }
        
        public bool IsFinished { get; set; }

        public Team Team1 { get; set; }
        
        public Team Team2 { get; set; }

        public int? TournamentId { get; set; }

        public int? ParentIndex { get; set; }
    }
}