namespace DataAccess.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string name;
        public List<Player> players;
        public Team() { }

        public Team(int id, string name, List<Player> players)
        {
            Id = id;
            this.name = name;
            this.players = players;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public List<Player> Players
        {
            get => players;
            set => players = value ?? new List<Player>();
        }
    }
}