using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BracketzApp.Models
{
    public class BracketScoreUpdateModel
    {
        public int BracketId { get; set; }
        public int ScoreTeam1 { get; set; }
        public int ScoreTeam2 { get; set; }
    }
}
