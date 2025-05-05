using DTO;

namespace Presentation.Models.Clubs
{
    public class TeamData
    {
        public int Id { get; set; }
        public List<Player> Players { get; set; }
        public string Name { get; set; }
        public string ClubAbbreviation { get; set; }
    }
}
