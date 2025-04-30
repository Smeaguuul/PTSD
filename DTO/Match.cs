namespace DTO
{
    public class Match
    {
        public Score Score { get; set; }
        public Team Opponent { get; set; }
        public DateOnly Date { get; set; }
        public Status Status { get; set; }
        public int Field { get; set; }
    }
}
