namespace DataAccess.Models
{
    public class Team
    {
        public int Id { get; set; }
        public List<Player> Players { get; set; }
        public string Name { get; set; }
        public Club Club { get; set; }

        public Team() { }

        public Team(int id, List<Player> players, string name, Club club)
        {
            Id = id;
            Players = players;
            Name = name;
            Club = club;
        }
    }
}