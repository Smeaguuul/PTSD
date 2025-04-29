using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    internal class Club
    {
        private string _name;
        public string Name { get { return _name; } }
        private string _abbriviation;
        public string Abbriviation
        {
            get { return _abbriviation; }
            set
            {
                if (value.Length > 3) throw new ArgumentException("The Abbriviation can't be more than 3 characters long");
                _abbriviation = value;
            }
        }
        private string _location;
        public string Location { get { return _location; } set { _location = value; } }

        private List<Team> _teams;
        public List<Team> Teams
        {
            get
            {
                return [.. _teams];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Club"/> class with the specified name, abbreviation, location, and teams.
        /// </summary>
        /// <param name="name">The name of the club.</param>
        /// <param name="abbriviation">The abbreviation of the club.</param>
        /// <param name="location">The location of the club.</param>
        /// <param name="teams">The list of teams associated with the club.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
        public Club(string name, string abbriviation, string location, List<Team> teams)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _abbriviation = abbriviation ?? throw new ArgumentNullException(nameof(abbriviation));
            _location = location ?? throw new ArgumentNullException(nameof(location));
            _teams = teams ?? throw new ArgumentNullException(nameof(teams));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Club"/> class with the specified name, abbreviation, and location.
        /// An empty list of teams will be created.
        /// </summary>
        /// <param name="name">The name of the club.</param>
        /// <param name="abbriviation">The abbreviation of the club.</param>
        /// <param name="location">The location of the club.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
        public Club(string name, string abbriviation, string location)
            : this(name, abbriviation, location, new List<Team>()) { }
    }
}
