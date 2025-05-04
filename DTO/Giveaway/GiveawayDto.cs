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
        private string Name { get; set; }
        private string Description { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }

        private List<ContestantDto> Contestants = new List<ContestantDto>();
    }
}
