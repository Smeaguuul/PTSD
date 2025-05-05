using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Giveaways
{
    public class GiveawayContestant
    {

        public int GiveawayId { get; set; }
        public Giveaway giveaway { get; set; }

        public int ContestantId { get; set; }
        public Contestant contestant { get; set; }

        public GiveawayContestant(Giveaway giveaway, Contestant contestant)
        {
            this.giveaway = giveaway;
            this.contestant = contestant;
            this.GiveawayId = giveaway.Id;
            this.ContestantId = contestant.Id;
        }
        public GiveawayContestant()
        {             // Default constructor for EF Core
        }

    }
}
