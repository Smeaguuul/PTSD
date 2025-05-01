namespace DTO
{
    public class Set
    {
        public int Id { get; set; }
        public bool Winner { get; set; }
        public List<Game> Games { get; set; }

        public Set() { }
    }
}