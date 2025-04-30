namespace DataAccess.Models
{
    public class Set
    {
        public int Id { get; set; }

        public bool? winner;
        public bool? Winner { get { return winner; } set { winner = value; } }
        public List<Game> games;
        public List<Game> Games { get { return games; } }

        public Set(int id, bool? winner, List<Game> games)
        {
            Id = id;
            this.winner = winner;
            this.games = games;
        }

        public Set()
        {
            games = new List<Game>();
        }

        public void AddGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            games.Add(game); //Calculate winner or something?
        }

    }
}