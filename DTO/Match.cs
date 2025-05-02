namespace DTO
{
    public class Match
    {
        public int Id { get; set; }
        public Score Score { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateOnly Date { get; set; }
        public Status Status { get; set; }
        public Field Field { get; set; }

        public Match(int id, Score score, Team homeTeam, Team awayTeam, DateOnly date, Status status)
        {
            Id = id;
            Score = score;
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Date = date;
            Status = status;
        }

        public Match() { } 
    }
}
