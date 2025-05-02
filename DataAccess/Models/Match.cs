using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Score Score { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateOnly Date { get; set; }
        public Status Status { get; set; }
        public int Field { get ; set ; }

        public Match() { }

        public Match(int id, Score score, Team homeTeam, Team awayTeam, DateOnly date, Status status, int field)
        {
            Id = id;
            Score = score;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Date = date;
            Status = status;
            Field = field;
        }
    }
}
