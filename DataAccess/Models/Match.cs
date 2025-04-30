using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Score Score { get; set; }
        public Team Opponent { get; set; }

        public DateOnly date;
        public DateOnly Date
        {
            get => date;
            set
            {
                //TODO: Maybe don't allow to "set" past dates? + Nullcheck - Mikkel
                date = value;
            }
        }
        public Status Status { get; set; }
   
        
        public int Field { get ; set ; }

        public Match() { }

        public Match(Score score, Team opponent, DateOnly date, Status status, int field)
        {
            this.Score = score;
            opponent = opponent;
            this.date = date;
            this.Status = status;
            this.Field = field;
        }

        public Match(Team opponent, DateOnly date, Status status, int field) 
            : this(opponent, date, status, field, new Score()) { }

        public Match(Team opponent, DateOnly date, Status status, int field, Score score)
        {
            opponent = opponent;
            this.date = date;
            this.Status = status;
            this.Field = field;
            this.Score = score;
        }
    }
}
