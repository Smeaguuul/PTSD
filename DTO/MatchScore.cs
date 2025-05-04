using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MatchScore
    {
        public string NameAway { get; set; }
        public string NameHome { get; set; }
        public string[] AwayPlayers { get; set; }
        public string[] HomePlayers { get; set; }
        public int SetsHome { get; set; }
        public int SetsAway { get; set; }
        public int GamesThisSetHome { get; set; }
        public int GamesThisSetAway { get; set; }
        public string PointsHome { get; set; }
        public string PointsAway { get; set; }
        public int FieldId { get; set; }          
    }
}
