namespace DataAccess.Models
{
    public class Set
    {
        public int Id { get; set; }
        public bool? Winner { get; set; }
        public List<Game> Games { get; set; } //Maximum of 13, right?

        public Set(int id, bool? winner, List<Game> games)
        {
            Id = id;
            this.Winner = winner;
            this.Games = games;
        }

        public Set()
        {
            Games = new List<Game>();
        }

        public void AddGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game), "Game cannot be null.");

            Games.Add(game); //Calculate winner or something?
        }

    }
}