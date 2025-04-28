namespace DTO
{
    public class Score
    {
        private List<Set> Sets { get; set; }

        private class Set
        {
            private  bool Winner { get; set; }
            private List<Game> Games { get; set; }

            private class Game
            {
                private bool Server { get; set; }
                private int number { get; set; }
                private List<bool> PointHistory { get; set; }
            }
        }
    }
}
