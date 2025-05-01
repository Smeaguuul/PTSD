namespace DTO
{
    public class Match
    {
        
        public Score Score { get; set; }
        public Team Team2 { get; set; }
        public Team Team1 { get; set; }
        public DateOnly Date { get; set; }
        public Status Status { get; set; }
        public Field Field {  get; set; }
        public int Id { get; set; }
        public Match(Team team2, Team team1, DateOnly date, Status status, int id)
        {
            Team2 = team2;
            Team1 = team1;
            Date = date;
            Status = status;
            Id = id;
        }
    }
}
