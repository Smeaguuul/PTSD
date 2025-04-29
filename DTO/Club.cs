using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Club
    {
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public string Location { get; set; }
        public string Abbriviation { get; set; }
    }
}
