using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Giveaway
{
    public class GiveawayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GiveawayStatus Status;

        private List<ContestantDto> Contestants = new List<ContestantDto>();
    }
}
