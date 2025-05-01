namespace DataAccess.Models
{
    public class Team
    {
        public int Id { get; set; }

        public List<Player> players;
        public string Name
        {
            get; set;
        }

        public Team() { }

        public Team(int id, string name, List<Player> players)
        {
            Id = id;
            this.Name = name;
            this.players = players;
        }

        
        public List<Player> Players
        {
            get => players;
            set => players = value ?? new List<Player>();
        }
    }
}