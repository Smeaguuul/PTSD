namespace DTO
{
    public class Player
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Player(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}