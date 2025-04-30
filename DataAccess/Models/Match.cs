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

        private Score _score;
        public Score Score { get => _score; }

        private Team _opponent;
        public Team Opponent { get => _opponent; }

        private DateOnly _date;
        public DateOnly Date
        {
            get => _date;
            set
            {
                //TODO: Maybe don't allow to "set" past dates? + Nullcheck - Mikkel
                _date = value;
            }
        }
        private Status _status;
        public Status Status {
            get => _status;
            set
            {
                if (value == null) throw new ArgumentNullException (nameof(value), "Status cannot be null.");
                _status = value;
            }
        }
        private int _field;
        public int Field
        {
            get => _field;
            set
            {
                if (value <= 0 || value >= 4)
                    throw new ArgumentException("Field must be 1, 2 or 3.");
                _field = value;
            }
        }

        public Match(Team opponent, DateOnly date, Status status, int field) 
            : this(opponent, date, status, field, new Score()) { }

        public Match(Team opponent, DateOnly date, Status status, int field, Score score)
        {
            _opponent = opponent;
            _date = date;
            _status = status;
            _field = field;
            _score = score;
        }
    }
}
