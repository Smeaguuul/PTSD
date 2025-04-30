namespace DataAccess.Models
{
    public class Team
    {
        public int Id { get; set; }

        private string _name;
        private List<Player> _players;
        public Team() { }

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