namespace DataAccess.Models
{
    public class Team
    {
        public int Id { get; set; }

        private string _name;
        private List<Player> _players;
        public Team() { }

        public Team(int id, string name, List<Player> players)
        {
            Id = id;
            _name = name;
            _players = players;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public List<Player> Players
        {
            get => _players;
            set => _players = value ?? new List<Player>();
        }
    }
}