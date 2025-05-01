namespace DTO
{
    public class Game(bool server, int number, List<bool> pointHistory)
    {
        public int Id;

        public bool Server { get; set; } = server;
        public int Number { get; set; } = number;
        public List<bool> PointHistory { get; set; } = pointHistory;

        
    }
}