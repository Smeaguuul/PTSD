namespace Business.Models
{
    internal class Team
    {
        private string _name;
        private List<Player> _players;

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