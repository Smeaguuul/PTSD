using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Giveaways
{
    public class Contestant
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public List<GiveawayContestant> GiveawayContestants { get; set; } = new List<GiveawayContestant>();

        public Contestant(string email)
        {
            Email = email;
        }

        public Contestant()
        {             // Default constructor for EF Core
        }

    }
}
