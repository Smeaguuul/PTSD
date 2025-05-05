using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Giveaways
{
    public class Giveaway
    {

        public int Id { get; set; }

        public Giveaway()
        {
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GiveawayStatus Status
        {
            get
            {
                if (StartDate > DateTime.Now)
                {
                    return GiveawayStatus.Upcoming;
                }
                else if (EndDate < DateTime.Now)
                {
                    return GiveawayStatus.Finished;
                }
                else
                {
                    return GiveawayStatus.Ongoing;
                }
            }
        }
        public List<GiveawayContestant> GiveawayContestants { get; set; } = new List<GiveawayContestant>();


        public Giveaway(string name, string description, DateTime startDate, DateTime endDate)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }

      






    }

    public enum GiveawayStatus
    {
        Upcoming,
        Ongoing,
        Finished
    }
}
