using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Club
    {
        public string Name { get; set; }

        private string _abbreviation;

        public string Abbreviation
        {
            get { return _abbreviation; }
            set
            {
                if (value.Length > 3) throw new ArgumentException("The Abbreviation can't be more than 3 characters long");
                _abbreviation = value;
            }
        }

        public string Location { get; set; }

        public List<Team> Teams { get; set; } = new List<Team>();

        public Club(string name, string abbreviation, string location, List<Team> teams)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Abbreviation = abbreviation ?? throw new ArgumentNullException(nameof(abbreviation));
            Location = location ?? throw new ArgumentNullException(nameof(location));
            Teams = teams ?? throw new ArgumentNullException(nameof(teams));
        }

        public Club(string name, string abbreviation, string location)
            : this(name, abbreviation, location, new List<Team>()) { }

        public Club() { }
    }


}
