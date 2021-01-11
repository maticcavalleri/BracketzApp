using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BracketzApp.Models
{
    public class MarkFinishedModel
    {
        public int BracketId { get; set; }
        
        public int ParentBracketIndex { get; set; }

        public int TournamentId { get; set; }
    }
}
