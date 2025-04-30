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
        public DateOnly Date { get; set; }
        public Status Status { get; set; }
        public int Field { get ; set ; }

        public Match() { }


    }
}
