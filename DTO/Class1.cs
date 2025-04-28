namespace DTO
{
    public class Match
    {
        private Score Score { get; set; }
        private DateOnly Date { get; set; }
        private Status Status { get; set; }
        private int Field { get; set; }

    }

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

    enum Status
    {
        Scheduled,
        Ongoing,
        Finished     
    }
}
