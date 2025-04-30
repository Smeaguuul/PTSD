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

        public Score score;
        public Score Score { get => score; }

        public Team opponent;
        public Team Opponent { get => opponent; }

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
        public Status status;
        public Status Status {
            get => status;
            set
            {
                if (value == null) throw new ArgumentNullException (nameof(value), "Status cannot be null.");
                status = value;
            }
        }
        public int Field { get ; set ; }

        public Match() { }

        public Match(Score score, Team opponent, DateOnly date, Status status, int field)
        {
            this.score = score;
            opponent = opponent;
            this.date = date;
            this.status = status;
            this.Field = field;
        }

        public Match(Team opponent, DateOnly date, Status status, int field) 
            : this(opponent, date, status, field, new Score()) { }

        public Match(Team opponent, DateOnly date, Status status, int field, Score score)
        {
            opponent = opponent;
            this.date = date;
            this.status = status;
            this.Field = field;
            this.score = score;
        }
    }
}
