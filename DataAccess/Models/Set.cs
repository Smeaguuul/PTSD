namespace DataAccess.Models
{
    public class Set
    {
        public int Id { get; set; }

        private bool? _winner;
        public bool? Winner { get { return _winner; } set { _winner = value; } }
        private List<Game> _games;
        public List<Game> Games { get { return _games; } }

        public Set(int id, bool? winner, List<Game> games)
        {
            Id = id;
            _winner = winner;
            _games = games;
        }

        public Set()
        {
            _games = new List<Game>();
        }

        public void AddGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            _games.Add(game); //Calculate winner or something?
        }

    }
}